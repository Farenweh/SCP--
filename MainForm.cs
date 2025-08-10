using System;
using System.Windows.Forms;

namespace ScpLauncher
{
    public partial class MainForm : Form
    {
        private TransferPage uploadPage;
        private TransferPage downloadPage;

        public MainForm()
        {
            InitializeComponent();
            CreatePages();
        }

        private void CreatePages()
        {
            uploadPage = new TransferPage(true) { Dock = DockStyle.Fill, Visible = false };
            downloadPage = new TransferPage(false) { Dock = DockStyle.Fill, Visible = false };
            this.Controls.Add(uploadPage);
            this.Controls.Add(downloadPage);
            this.Controls.SetChildIndex(uploadPage, 0);
            this.Controls.SetChildIndex(downloadPage, 0);
        }

        private void ShowHome()
        {
            uploadPage.Visible = false;
            downloadPage.Visible = false;
            panelMain.Visible = true;
        }

        private void ShowUpload()
        {
            panelMain.Visible = false;
            downloadPage.Visible = false;
            uploadPage.Visible = true;
            uploadPage.Focus();
        }

        private void ShowDownload()
        {
            panelMain.Visible = false;
            uploadPage.Visible = false;
            downloadPage.Visible = true;
            downloadPage.Focus();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            ShowUpload();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            ShowDownload();
        }

        internal void BackToHome()
        {
            ShowHome();
        }
    }
}
