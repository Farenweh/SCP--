# SCP--

Windows 上的轻量级 SCP 图形工具：维护多套连接/路径预设，一键上传/下载，拖拽即用，并提供即时校验与占位提示。基于.NET 4.8.1构建，在具备openSSH的Win 10/11上开箱即用。

![示例截图](./example.png)

## 功能
- 预设管理：新增/修改/重命名/删除，多配置共存（`~/.SCP--/*.cfg`）
- 上传/下载双页面：文件/目录模式切换
- 即时校验：
    - 私钥文件是否存在
    - 下载页面的本地目录是否可写（存在时）
    - 警告在页面内非弹出显示
- 拖拽填充：支持将私钥文件、本地路径直接拖入
- 智能端口：仅在填写端口时添加 `-P` 参数
- 细节优化：占位文本、按钮冷却、窗口尺寸/布局优化

## 预设配置

应用会在启动时创建配置目录：`~/.SCP--/`

每个预设是一个 `别名.cfg`，内容是 key=value 文本，例如：

```
username=your_username
ip=your_server_ip
port=22
keypath=C:\\Users\\you\\.ssh\\id_rsa
downloadlocal=C:\\Users\\you\\Downloads
uploadremote=~/workspace
```

在上传/下载页选择“配置预设”即可快速填充；保存时会记住上次所选预设，下次进入自动选中。

## 构建与运行

- Debug 版：`bin/Debug/net481/SCP--.exe`（便于调试）
- Release 版：`bin/Release/net481/SCP--.exe`（用于分发）

工作区提供 `build.bat`，默认以 Release 构建。

提示：若构建提示文件被占用，请先关闭正在运行的 EXE 后再编译。

## 字段说明

- 用户名：SSH 用户
- 远端 IP：服务器 IP/域名
- 远端端口：留空则使用默认端口（不会附加 -P）
- SSH 密钥路径：私钥文件完整路径
- 本地路径：下载时为保存目录；上传时为文件/目录路径
- 远端路径：对应上传/下载的服务器端路径

## 许可与致谢

- 作者：chenbuyi
- GitHub：https://github.com/Farenweh
- 许可：仅供学习与个人使用

如果觉得有用，欢迎 Star 支持！
