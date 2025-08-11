using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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
                txtKey.Text = cfg.KeyPath ?? string.Empty;
                txtIp.Text = cfg.Ip ?? string.Empty;
                txtPort.Text = cfg.Port ?? string.Empty;
                if (uploadMode)
                {
                    txtRemote.Text = cfg.UploadRemote ?? string.Empty;
                    txtLocal.Text = string.Empty; // 上传时由用户选择
                }
                else
                {
                    txtRemote.Text = string.Empty; // 下载时由用户填写
                    txtLocal.Text = cfg.DownloadLocal ?? string.Empty;
                }
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
                Port = txtPort.Text.Trim(),
                KeyPath = txtKey.Text.Trim(),
                DownloadLocal = txtLocal.Text.Trim(),
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
                    txtKey.Text = ofd.FileName;
                }
            }
        }

        private void btnChooseLocal_Click(object sender, EventArgs e)
        {
            // 下载模式：本地路径始终选择目录，用于存放下载内容
            if (!uploadMode)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog(this) == DialogResult.OK)
                    {
                        txtLocal.Text = fbd.SelectedPath;
                    }
                }
                return;
            }

            // 上传模式：根据“传输类型”决定选择文件或目录
            if (radioDir.Checked)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog(this) == DialogResult.OK)
                    {
                        txtLocal.Text = fbd.SelectedPath;
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
                        txtLocal.Text = ofd.FileName;
                    }
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLocal.Text) || string.IsNullOrWhiteSpace(txtRemote.Text))
                return;

            // 下载模式下，本地路径必须为目录
            if (!uploadMode)
            {
                var path = txtLocal.Text.Trim();
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
                    TransferConfig baseCfg = new TransferConfig();
                    if (ConfigStore.Exists(alias))
                    {
                        // 已存在则合并
                        baseCfg = ConfigStore.Load(alias);
                    }
                    var curr = CollectConfigFromFields();
                    // 通用字段
                    baseCfg.Username = curr.Username;
                    baseCfg.Ip = curr.Ip;
                    baseCfg.Port = curr.Port;
                    baseCfg.KeyPath = curr.KeyPath;
                    // 页面特定字段
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
            // 通用字段
            baseCfg.Username = curr.Username;
            baseCfg.Ip = curr.Ip;
            baseCfg.Port = curr.Port;
            baseCfg.KeyPath = curr.KeyPath;
            // 页面特定字段
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
            var port = txtPort.Text.Trim();
            var key = txtKey.Text.Trim();
            var local = txtLocal.Text.Trim();
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
            // start a new cmd window and keep it open
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
    }
}
