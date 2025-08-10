namespace ScpLauncher
{
    partial class TransferPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblHeader = new System.Windows.Forms.Label();
            this.radioFile = new System.Windows.Forms.RadioButton();
            this.radioDir = new System.Windows.Forms.RadioButton();
            this.lblType = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.panelScroll = new System.Windows.Forms.Panel();
            this.table = new System.Windows.Forms.TableLayoutPanel();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblKey = new System.Windows.Forms.Label();
            this.lblLocal = new System.Windows.Forms.Label();
            this.lblRemote = new System.Windows.Forms.Label();
            this.lblIp = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.txtLocal = new System.Windows.Forms.TextBox();
            this.txtRemote = new System.Windows.Forms.TextBox();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnChooseKey = new System.Windows.Forms.Button();
            this.btnChooseLocal = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.panelScroll.SuspendLayout();
            this.table.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(640, 52);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "标题";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.radioFile);
            this.panelTop.Controls.Add(this.radioDir);
            this.panelTop.Controls.Add(this.lblType);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 52);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(24, 6, 24, 6);
            this.panelTop.Size = new System.Drawing.Size(640, 40);
            this.panelTop.TabIndex = 1;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(20, 10);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(68, 15);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "传输类型:";
            // 
            // radioFile
            // 
            this.radioFile.AutoSize = true;
            this.radioFile.Checked = true;
            this.radioFile.Location = new System.Drawing.Point(110, 8);
            this.radioFile.Name = "radioFile";
            this.radioFile.Size = new System.Drawing.Size(50, 19);
            this.radioFile.TabIndex = 1;
            this.radioFile.TabStop = true;
            this.radioFile.Text = "文件";
            this.radioFile.UseVisualStyleBackColor = true;
            // 
            // radioDir
            // 
            this.radioDir.AutoSize = true;
            this.radioDir.Location = new System.Drawing.Point(170, 8);
            this.radioDir.Name = "radioDir";
            this.radioDir.Size = new System.Drawing.Size(50, 19);
            this.radioDir.TabIndex = 2;
            this.radioDir.Text = "目录";
            this.radioDir.UseVisualStyleBackColor = true;
            // 
            // panelScroll
            // 
            this.panelScroll.AutoScroll = true;
            this.panelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScroll.Location = new System.Drawing.Point(0, 92);
            this.panelScroll.Name = "panelScroll";
            this.panelScroll.Padding = new System.Windows.Forms.Padding(24, 8, 24, 8);
            this.panelScroll.Size = new System.Drawing.Size(640, 212);
            this.panelScroll.TabIndex = 5;
            this.panelScroll.Controls.Add(this.table);
            // 
            // table
            // 
            this.table.AutoSize = false;
            this.table.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.table.Dock = System.Windows.Forms.DockStyle.Top;
            this.table.ColumnCount = 3;
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.table.Controls.Add(this.lblUser, 0, 0);
            this.table.Controls.Add(this.lblKey, 0, 1);
            this.table.Controls.Add(this.lblLocal, 0, 2);
            this.table.Controls.Add(this.lblRemote, 0, 3);
            this.table.Controls.Add(this.lblIp, 0, 4);
            this.table.Controls.Add(this.lblPort, 0, 5);
            this.table.Controls.Add(this.txtUser, 1, 0);
            this.table.Controls.Add(this.txtKey, 1, 1);
            this.table.Controls.Add(this.txtLocal, 1, 2);
            this.table.Controls.Add(this.txtRemote, 1, 3);
            this.table.Controls.Add(this.txtIp, 1, 4);
            this.table.Controls.Add(this.txtPort, 1, 5);
            this.table.Controls.Add(this.btnChooseKey, 2, 1);
            this.table.Controls.Add(this.btnChooseLocal, 2, 2);
            this.table.Location = new System.Drawing.Point(24, 8);
            this.table.Name = "table";
            this.table.RowCount = 6;
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.table.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.table.Size = new System.Drawing.Size(592, 240);
            this.table.TabIndex = 2;
            // 
            // Labels
            // 
            this.lblUser.Text = "用户名:";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblKey.Text = "SSH 密钥路径:";
            this.lblKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLocal.Text = "本地路径:";
            this.lblLocal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblRemote.Text = "远端路径:";
            this.lblRemote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblIp.Text = "远端 IP:";
            this.lblIp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPort.Text = "远端端口:";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxes
            // 
            this.txtUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUser.Margin = new System.Windows.Forms.Padding(3);
            this.txtKey.Margin = new System.Windows.Forms.Padding(3);
            this.txtLocal.Margin = new System.Windows.Forms.Padding(3);
            this.txtRemote.Margin = new System.Windows.Forms.Padding(3);
            this.txtIp.Margin = new System.Windows.Forms.Padding(3);
            this.txtPort.Margin = new System.Windows.Forms.Padding(3);
            // 
            // Buttons in table
            // 
            this.btnChooseKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnChooseKey.Margin = new System.Windows.Forms.Padding(6, 4, 0, 4);
            this.btnChooseKey.Text = "选择…";
            this.btnChooseKey.Click += new System.EventHandler(this.btnChooseKey_Click);
            this.btnChooseLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnChooseLocal.Margin = new System.Windows.Forms.Padding(6, 4, 0, 4);
            this.btnChooseLocal.Text = "选择…";
            this.btnChooseLocal.Click += new System.EventHandler(this.btnChooseLocal_Click);
            // 
            // panelButtons
            // 
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelButtons.Location = new System.Drawing.Point(0, 304);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(0, 6, 24, 4);
            this.panelButtons.Size = new System.Drawing.Size(640, 56);
            this.panelButtons.TabIndex = 3;
            this.panelButtons.WrapContents = false;
            // add controls after layout to ensure correct order
            this.panelButtons.Controls.Add(this.btnStart);
            this.panelButtons.Controls.Add(this.btnBack);
            // 
            // btnBack
            // 
            this.btnBack.AutoSize = false;
            this.btnBack.Text = "返回";
            this.btnBack.Margin = new System.Windows.Forms.Padding(8, 6, 0, 6);
            this.btnBack.Size = new System.Drawing.Size(120, 40);
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnStart
            // 
            this.btnStart.AutoSize = false;
            this.btnStart.Text = "开始";
            this.btnStart.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnStart.Size = new System.Drawing.Size(120, 40);
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // TransferPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelScroll);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.lblHeader);
            this.Name = "TransferPage";
            this.Size = new System.Drawing.Size(640, 360);
            this.panelScroll.ResumeLayout(false);
            this.panelScroll.PerformLayout();
            this.table.ResumeLayout(false);
            this.table.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.RadioButton radioFile;
        private System.Windows.Forms.RadioButton radioDir;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.TableLayoutPanel table;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.Label lblLocal;
        private System.Windows.Forms.Label lblRemote;
        private System.Windows.Forms.Label lblIp;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.TextBox txtLocal;
        private System.Windows.Forms.TextBox txtRemote;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnChooseKey;
        private System.Windows.Forms.Button btnChooseLocal;
    private System.Windows.Forms.FlowLayoutPanel panelButtons;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnBack;
    private System.Windows.Forms.Panel panelScroll;
    }
}
