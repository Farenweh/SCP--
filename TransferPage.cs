using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ScpLauncher
{
    public partial class TransferPage : UserControl
    {
        private readonly bool uploadMode;
        private int countdown = 0;
        private readonly Timer timer;
        private bool suppressApply = false;

        public TransferPage(bool upload)
        {
            uploadMode = upload;
            InitializeComponent();
            lblHeader.Text = uploadMode ? "SCP - 上传" : "SCP - 下载";

            // 初始化配置目录并刷新预设列表
            ConfigStore.EnsureConfigDir();
            RefreshConfigList();

            btnStart.Text = uploadMode ? "开始上传" : "开始下载";

            timer = new Timer { Interval = 1000 };
            timer.Tick += (_, __) => TickCountdown();

            // Drag & Drop support
            try
            {
                txtKey.AllowDrop = true;
                txtKey.DragEnter += OnDragEnterFiles;
                txtKey.DragDrop += OnKeyDragDrop;

                txtLocal.AllowDrop = true;
                txtLocal.DragEnter += OnDragEnterFiles;
                txtLocal.DragDrop += OnLocalDragDrop;
            }
            catch { }

            // 占位提示（托管方式，浅灰色）
            SetupPlaceholder(txtPort, "22");
            SetupPlaceholder(txtKey, "拖拽到此处");
            SetupPlaceholder(txtLocal, "拖拽到此处");
            RefreshPlaceholders();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var parent = this.FindForm() as MainForm;
            parent?.BackToHome();
        }

        private void RefreshConfigList(string selectAlias = null)
        {
            var aliases = new System.Collections.Generic.List<string>(ConfigStore.ListAliases());
            comboConfig.Items.Clear();
            if (aliases.Count == 0)
            {
                comboConfig.Items.Add("(无配置)");
                comboConfig.SelectedIndex = 0;
                ApplyConfig(new TransferConfig());
                return;
            }
            foreach (var a in aliases)
                comboConfig.Items.Add(a);
            if (!string.IsNullOrWhiteSpace(selectAlias) && aliases.Contains(selectAlias))
                comboConfig.SelectedItem = selectAlias;
            else
                comboConfig.SelectedIndex = 0;
        }

        private void ApplyConfig(TransferConfig cfg)
        {
            suppressApply = true;
            try
            {
                txtUser.Text = cfg.Username ?? string.Empty;
                txtIp.Text = cfg.Ip ?? string.Empty;
                // 对带占位符的文本框使用安全写入，避免残留灰显状态
                SetTextValue(txtKey, cfg.KeyPath);
                SetTextValue(txtPort, cfg.Port);
                if (uploadMode)
                {
                    txtRemote.Text = cfg.UploadRemote ?? string.Empty;
                    SetTextValue(txtLocal, string.Empty); // 上传由用户选择
                }
                else
                {
                    txtRemote.Text = string.Empty; // 下载由用户填写
                    SetTextValue(txtLocal, cfg.DownloadLocal);
                }
                RefreshPlaceholders();
            }
            finally
            {
                suppressApply = false;
            }
        }

        private TransferConfig CollectConfigFromFields()
        {
            return new TransferConfig
            {
                Username = txtUser.Text.Trim(),
                Ip = txtIp.Text.Trim(),
                Port = ReadTextValue(txtPort).Trim(),
                KeyPath = ReadTextValue(txtKey).Trim(),
                DownloadLocal = ReadTextValue(txtLocal).Trim(),
                UploadRemote = txtRemote.Text.Trim()
            };
        }

        private void btnChooseKey_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "选择私钥文件";
                ofd.Filter = "所有文件 (*.*)|*.*";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    SetTextValue(txtKey, ofd.FileName);
                }
            }
        }

        private void btnChooseLocal_Click(object sender, EventArgs e)
        {
            // 下载模式：本地路径始终选择目录
            if (!uploadMode)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog(this) == DialogResult.OK)
                    {
                        SetTextValue(txtLocal, fbd.SelectedPath);
                    }
                }
                return;
            }

            // 上传模式：根据传输类型
            if (radioDir.Checked)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog(this) == DialogResult.OK)
                    {
                        SetTextValue(txtLocal, fbd.SelectedPath);
                    }
                }
            }
            else
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Title = "选择文件";
                    ofd.Filter = "所有文件 (*.*)|*.*";
                    if (ofd.ShowDialog(this) == DialogResult.OK)
                    {
                        SetTextValue(txtLocal, ofd.FileName);
                    }
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReadTextValue(txtLocal)) || string.IsNullOrWhiteSpace(txtRemote.Text))
                return;

            if (!uploadMode)
            {
                var path = ReadTextValue(txtLocal).Trim();
                if (File.Exists(path))
                {
                    MessageBox.Show(this, "下载任务的本地路径用于保存内容，请选择一个目录。", "路径错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!Directory.Exists(path))
                {
                    var r = MessageBox.Show(this, $"目录不存在，是否创建？\n{path}", "创建目录", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (r == DialogResult.Yes)
                    {
                        try { Directory.CreateDirectory(path); }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, ex.Message, "创建目录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }

            string cmd = BuildScpCommand();
            LaunchInCmd(cmd);
            LockButton();
        }

        private void comboConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressApply) return;
            var alias = comboConfig.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(alias) || alias == "(无配置)")
            {
                ApplyConfig(new TransferConfig());
                return;
            }
            var cfg = ConfigStore.Load(alias);
            ApplyConfig(cfg);
        }

        private void btnCfgAdd_Click(object sender, EventArgs e)
        {
            using (var prompt = new InputBox("输入新配置别名:"))
            {
                if (prompt.ShowDialog(this) == DialogResult.OK)
                {
                    var alias = prompt.Value?.Trim();
                    if (string.IsNullOrWhiteSpace(alias)) return;
                    TransferConfig baseCfg = ConfigStore.Exists(alias) ? ConfigStore.Load(alias) : new TransferConfig();
                    var curr = CollectConfigFromFields();
                    baseCfg.Username = curr.Username;
                    baseCfg.Ip = curr.Ip;
                    baseCfg.Port = curr.Port;
                    baseCfg.KeyPath = curr.KeyPath;
                    if (uploadMode)
                        baseCfg.UploadRemote = curr.UploadRemote;
                    else
                        baseCfg.DownloadLocal = curr.DownloadLocal;
                    ConfigStore.Save(alias, baseCfg);
                    RefreshConfigList(alias);
                }
            }
        }

        private void btnCfgEdit_Click(object sender, EventArgs e)
        {
            var alias = comboConfig.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(alias) || alias == "(无配置)") return;
            var baseCfg = ConfigStore.Load(alias);
            var curr = CollectConfigFromFields();
            baseCfg.Username = curr.Username;
            baseCfg.Ip = curr.Ip;
            baseCfg.Port = curr.Port;
            baseCfg.KeyPath = curr.KeyPath;
            if (uploadMode)
                baseCfg.UploadRemote = curr.UploadRemote;
            else
                baseCfg.DownloadLocal = curr.DownloadLocal;
            ConfigStore.Save(alias, baseCfg);
            MessageBox.Show(this, "已保存", "提示");
        }

        private void btnCfgDelete_Click(object sender, EventArgs e)
        {
            var alias = comboConfig.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(alias) || alias == "(无配置)") return;
            if (MessageBox.Show(this, $"确定删除配置 '{alias}'?", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ConfigStore.Delete(alias);
                RefreshConfigList();
            }
        }

        private void btnCfgRename_Click(object sender, EventArgs e)
        {
            var alias = comboConfig.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(alias) || alias == "(无配置)") return;
            using (var prompt = new InputBox("输入新别名:", "重命名配置", alias))
            {
                if (prompt.ShowDialog(this) == DialogResult.OK)
                {
                    var newAlias = prompt.Value == null ? null : prompt.Value.Trim();
                    if (string.IsNullOrWhiteSpace(newAlias) || string.Equals(newAlias, alias, StringComparison.OrdinalIgnoreCase)) return;
                    try
                    {
                        ConfigStore.Rename(alias, newAlias);
                        RefreshConfigList(newAlias);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "重命名失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private string BuildScpCommand()
        {
            var user = txtUser.Text.Trim();
            var ip = txtIp.Text.Trim();
            var port = ReadTextValue(txtPort).Trim();
            var key = ReadTextValue(txtKey).Trim();
            var local = ReadTextValue(txtLocal).Trim();
            var remote = txtRemote.Text.Trim();
            bool isDir = radioDir.Checked;
            string flagR = isDir ? "-r" : string.Empty;

            string portArg = string.IsNullOrWhiteSpace(port) ? string.Empty : $"-P {port}";
            string baseCmd = $"scp -i \"{key}\" {portArg} {flagR}".Trim();
            string src, dst;
            if (uploadMode)
            {
                src = $"\"{local}\"";
                dst = $"{user}@{ip}:\"{remote}\"";
            }
            else
            {
                src = $"{user}@{ip}:\"{remote}\"";
                dst = $"\"{local}\"";
            }
            return $"{baseCmd} {src} {dst}";
        }

        private void LaunchInCmd(string cmd)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c start \"\" cmd /k \"{cmd}\"",
                UseShellExecute = false,
                CreateNoWindow = false
            };
            try
            {
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "启动失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LockButton()
        {
            btnStart.Enabled = false;
            countdown = 3;
            btnStart.Text = $"{countdown}s";
            timer.Start();
        }

        private void TickCountdown()
        {
            countdown--;
            if (countdown <= 0)
            {
                timer.Stop();
                btnStart.Enabled = true;
                btnStart.Text = uploadMode ? "开始上传" : "开始下载";
            }
            else
            {
                btnStart.Text = $"{countdown}s";
            }
        }

        private void OnDragEnterFiles(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void OnKeyDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null || !e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files == null || files.Length == 0) return;
            var path = files[0];
            if (Directory.Exists(path)) return; // 只接受文件
            SetTextValue(txtKey, path);
        }

        private void OnLocalDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null || !e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files == null || files.Length == 0) return;
            var path = files[0];

            if (uploadMode)
            {
                if (Directory.Exists(path)) radioDir.Checked = true; else radioFile.Checked = true;
                txtLocal.Text = path;
            }
            else
            {
                string targetDir = path;
                if (File.Exists(path))
                {
                    var parent = Path.GetDirectoryName(path);
                    if (!string.IsNullOrEmpty(parent)) targetDir = parent;
                }
                else if (!Directory.Exists(path))
                {
                    return;
                }
                SetTextValue(txtLocal, targetDir);
            }
        }

        // --- 占位符工具 ---
        private class PlaceholderState
        {
            public string Placeholder;
            public bool Active;
            public Color OriginalColor;
        }

        private void SetupPlaceholder(TextBox tb, string placeholder)
        {
            if (tb == null) return;
            var state = new PlaceholderState { Placeholder = placeholder, Active = false, OriginalColor = tb.ForeColor };
            tb.Tag = state;
            tb.Enter += (s, e) =>
            {
                var st = tb.Tag as PlaceholderState;
                if (st != null && st.Active)
                {
                    st.Active = false;
                    tb.Text = string.Empty;
                    tb.ForeColor = st.OriginalColor;
                }
            };
            tb.Leave += (s, e) => ApplyPlaceholderIfEmpty(tb);
        }

        private void ApplyPlaceholderIfEmpty(TextBox tb)
        {
            var st = tb.Tag as PlaceholderState;
            if (st == null) return;
            if (string.IsNullOrEmpty(tb.Text) && !tb.Focused)
            {
                st.Active = true;
                tb.ForeColor = Color.FromArgb(150, 150, 150);
                tb.Text = st.Placeholder;
            }
        }

        private void RefreshPlaceholders()
        {
            ApplyPlaceholderIfEmpty(txtPort);
            ApplyPlaceholderIfEmpty(txtKey);
            ApplyPlaceholderIfEmpty(txtLocal);
        }

        private string ReadTextValue(TextBox tb)
        {
            var st = tb.Tag as PlaceholderState;
            if (st != null && st.Active) return string.Empty;
            if (st != null && tb.Text == st.Placeholder && tb.ForeColor.ToArgb() == Color.FromArgb(150, 150, 150).ToArgb())
            {
                return string.Empty;
            }
            return tb.Text ?? string.Empty;
        }

        // 程序化设置值时，关闭占位符状态并恢复原始前景色
        private void SetTextValue(TextBox tb, string value)
        {
            var st = tb.Tag as PlaceholderState;
            if (st != null)
            {
                st.Active = false;
                tb.ForeColor = st.OriginalColor;
            }
            tb.Text = value ?? string.Empty;
        }
    }
}
