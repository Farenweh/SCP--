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

        public TransferPage(bool upload)
        {
            uploadMode = upload;
            InitializeComponent();
            lblHeader.Text = uploadMode ? "SCP - 上传" : "SCP - 下载";

            // defaults
            txtUser.Text = Defaults.Username;
            txtKey.Text = Defaults.KeyPath;
            txtIp.Text = Defaults.Ip;
            txtPort.Text = Defaults.Port;
            txtRemote.Text = uploadMode ? Defaults.UploadRemote : string.Empty;
            txtLocal.Text = uploadMode ? string.Empty : Defaults.DownloadLocal;

            timer = new Timer { Interval = 1000 };
            timer.Tick += (_, __) => TickCountdown();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var parent = this.FindForm() as MainForm;
            parent?.BackToHome();
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

            string cmd = BuildScpCommand();
            LaunchInCmd(cmd);
            LockButton();
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

            string baseCmd = $"scp -i \"{key}\" -P {port} {flagR}".Trim();
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
