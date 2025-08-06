#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
SCP-- 懒狗 GUI-SCP 启动器
"""
import sys
import subprocess
from PyQt6.QtCore import Qt, QTimer
from PyQt6.QtWidgets import (
    QApplication, QWidget, QLabel, QPushButton, QStackedWidget, QVBoxLayout,
    QHBoxLayout, QLineEdit, QFileDialog, QRadioButton, QButtonGroup, QFormLayout
)

# --------------------------- 默认值 --------------------------- #
DEFAULTS = {
    "username": "chenbuyi103",
    "ip": "10.68.44.250",
    "port": "10342",
    "key": r"C:\Users\Buyi\.ssh\id_rsa_lap",
    "download_local": r"C:\Users\Buyi\Desktop",
    "upload_remote": "~/workspace",
    "title": "SCP-- 启动器 @ chenbuyi103",
}
FOOTER_TEXT = "© chenbuyi  –  https://github.com/Farenweh"


# ================================================================= #
#                             工具函数                               #
# ================================================================= #
def footer_label() -> QLabel:
    """生成一个半透明的小字标签，用于版权信息。"""
    lbl = QLabel(FOOTER_TEXT, alignment=Qt.AlignmentFlag.AlignCenter)
    lbl.setStyleSheet(
        "font-size:10px;"
        "color: rgba(0,0,0,0.35);"          # 半透明
    )
    return lbl


# ================================================================= #
#                               主窗口                               #
# ================================================================= #
class MainWindow(QWidget):
    def __init__(self):
        super().__init__()
        self.setWindowTitle(DEFAULTS["title"])
        self.resize(540, 340)

        self.stack = QStackedWidget(self)
        self._build_first_page()
        self._build_transfer_pages()

        lay = QVBoxLayout(self)
        lay.addWidget(self.stack)

    # ---------------------- 首页 ---------------------- #
    def _build_first_page(self):
        page = QWidget()
        v = QVBoxLayout(page)
        v.addStretch()

        lab = QLabel("请选择操作类型", alignment=Qt.AlignmentFlag.AlignCenter)
        lab.setStyleSheet("font-size: 22px;")
        v.addWidget(lab)

        btn_upload = QPushButton("上传")
        btn_download = QPushButton("下载")
        btn_upload.clicked.connect(lambda: self.stack.setCurrentIndex(1))
        btn_download.clicked.connect(lambda: self.stack.setCurrentIndex(2))

        for b in (btn_upload, btn_download):
            b.setMinimumHeight(44)
            v.addWidget(b)

        v.addStretch()
        v.addWidget(footer_label())
        self.stack.addWidget(page)

    # ----------------- 上传 / 下载页面 ----------------- #
    def _build_transfer_pages(self):
        self.upload_page = TransferPage(self.stack, upload_mode=True)
        self.download_page = TransferPage(self.stack, upload_mode=False)
        self.stack.addWidget(self.upload_page)
        self.stack.addWidget(self.download_page)


# ================================================================= #
#                       上传 / 下载 共用页面                         #
# ================================================================= #
class TransferPage(QWidget):
    def __init__(self, stacked: QStackedWidget, upload_mode: bool):
        super().__init__()
        self.stacked = stacked
        self.upload_mode = upload_mode
        self._build_ui()

    # ------------------ UI 构建 ------------------ #
    def _build_ui(self):
        title = "上传" if self.upload_mode else "下载"
        self.setWindowTitle(f"SCP - {title}")
        main = QVBoxLayout(self)

        # 1) 传输类型单选
        radio_file = QRadioButton("文件")
        radio_dir = QRadioButton("目录")
        radio_file.setChecked(True)
        self.radio_group = QButtonGroup(self)
        self.radio_group.addButton(radio_file, 0)
        self.radio_group.addButton(radio_dir, 1)

        bar_type = QHBoxLayout()
        bar_type.addWidget(QLabel("传输类型:"))
        bar_type.addWidget(radio_file)
        bar_type.addWidget(radio_dir)
        bar_type.addStretch()
        main.addLayout(bar_type)

        # 2) 表单
        form = QFormLayout()
        self.edit_user = QLineEdit(DEFAULTS["username"])
        self.edit_key = QLineEdit(DEFAULTS["key"])
        self.edit_ip = QLineEdit(DEFAULTS["ip"])
        self.edit_port = QLineEdit(DEFAULTS["port"])
        default_remote = DEFAULTS["upload_remote"] if self.upload_mode else ""
        self.edit_remote = QLineEdit(default_remote)

        # 私钥选择
        btn_key = QPushButton("选择…")
        btn_key.clicked.connect(self._choose_key)
        box_key = QHBoxLayout()
        box_key.addWidget(self.edit_key)
        box_key.addWidget(btn_key)

        # 本地路径
        default_local = "" if self.upload_mode else DEFAULTS["download_local"]
        self.edit_local = QLineEdit(default_local)
        btn_local = QPushButton("选择…")
        btn_local.clicked.connect(self._choose_local)
        box_local = QHBoxLayout()
        box_local.addWidget(self.edit_local)
        box_local.addWidget(btn_local)

        form.addRow("用户名:", self.edit_user)
        form.addRow("SSH 密钥路径:", box_key)
        form.addRow("本地路径:", box_local)
        form.addRow("远端路径:", self.edit_remote)
        form.addRow("远端 IP:", self.edit_ip)
        form.addRow("远端端口:", self.edit_port)
        main.addLayout(form)

        # 3) 控制按钮
        ctrl = QHBoxLayout()
        btn_back = QPushButton("返回")
        btn_back.clicked.connect(lambda: self.stacked.setCurrentIndex(0))
        self.btn_start = QPushButton(f"开始{title}")
        self.btn_start.clicked.connect(self._start_transfer)

        ctrl.addStretch()
        ctrl.addWidget(btn_back)
        ctrl.addWidget(self.btn_start)
        main.addLayout(ctrl)

        # 4) 版权 footer
        main.addStretch()
        main.addWidget(footer_label())

    # ----------- 路径选择 ----------- #
    def _choose_key(self):
        path, _ = QFileDialog.getOpenFileName(self, "选择私钥文件", "", "所有文件 (*)")
        if path:
            self.edit_key.setText(path)

    def _choose_local(self):
        choose_dir = self.radio_group.checkedId() == 1
        if choose_dir:
            path = QFileDialog.getExistingDirectory(self, "选择目录", "")
        else:
            path, _ = QFileDialog.getOpenFileName(self, "选择文件", "", "所有文件 (*)")
        if path:
            self.edit_local.setText(path)

    # ----------- 传输逻辑 ----------- #
    def _start_transfer(self):
        if not self.edit_local.text().strip() or not self.edit_remote.text().strip():
            return  # 静默忽略

        cmd = self._build_scp_command()
        subprocess.Popen(f'start "" cmd /k "{cmd}"', shell=True)
        self._lock_button()

    def _build_scp_command(self) -> str:
        user = self.edit_user.text().strip()
        ip = self.edit_ip.text().strip()
        port = self.edit_port.text().strip()
        key = self.edit_key.text().strip()
        local = self.edit_local.text().strip()
        remote = self.edit_remote.text().strip()

        is_dir = self.radio_group.checkedId() == 1
        flag_r = "-r" if is_dir else ""
        base = f'scp -i "{key}" -P {port} {flag_r}'.strip()

        if self.upload_mode:
            src = f'"{local}"'
            dst = f'{user}@{ip}:"{remote}"'
        else:
            src = f'{user}@{ip}:"{remote}"'
            dst = f'"{local}"'
        return f'{base} {src} {dst}'

    # ----------- 按钮锁定 ----------- #
    def _lock_button(self):
        self.btn_start.setEnabled(False)
        self._count = 3
        self.btn_start.setText(f"{self._count}s")
        self._timer = QTimer(self)
        self._timer.setInterval(1000)
        self._timer.timeout.connect(self._tick)
        self._timer.start()

    def _tick(self):
        self._count -= 1
        if self._count == 0:
            self._timer.stop()
            self.btn_start.setEnabled(True)
            self.btn_start.setText(f"开始{'上传' if self.upload_mode else '下载'}")
        else:
            self.btn_start.setText(f"{self._count}s")


# ================================================================= #
#                               入口                                #
# ================================================================= #
def main():
    app = QApplication(sys.argv)
    win = MainWindow()
    win.show()
    sys.exit(app.exec())


if __name__ == "__main__":
    main()