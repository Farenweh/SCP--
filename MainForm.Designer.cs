namespace ScpLauncher
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelMain = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.layoutButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.lblFooter = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.layoutButtons);
            this.panelMain.Controls.Add(this.lblTitle);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(24, 24, 24, 24);
            this.panelMain.Size = new System.Drawing.Size(640, 338);
            this.panelMain.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.lblTitle.Location = new System.Drawing.Point(24, 24);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(592, 60);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "请选择操作类型";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutButtons
            // 
            this.layoutButtons.ColumnCount = 1;
            this.layoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutButtons.Controls.Add(this.btnUpload, 0, 0);
            this.layoutButtons.Controls.Add(this.btnDownload, 0, 1);
            this.layoutButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutButtons.Location = new System.Drawing.Point(24, 84);
            this.layoutButtons.Name = "layoutButtons";
            this.layoutButtons.RowCount = 2;
            this.layoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutButtons.Padding = new System.Windows.Forms.Padding(0);
            this.layoutButtons.Size = new System.Drawing.Size(592, 230);
            this.layoutButtons.TabIndex = 4;
            // 
            // btnUpload
            // 
            this.btnUpload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUpload.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btnUpload.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.TabIndex = 1;
            this.btnUpload.Text = "上传";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDownload.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btnDownload.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "下载";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // lblFooter
            // 
            this.lblFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblFooter.ForeColor = System.Drawing.Color.FromArgb(120, 120, 120);
            this.lblFooter.Location = new System.Drawing.Point(0, 338);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(640, 22);
            this.lblFooter.TabIndex = 3;
            this.lblFooter.Text = Defaults.Footer;
            this.lblFooter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.lblFooter);
            this.Controls.Add(this.panelMain);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = Defaults.Title;
            this.panelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
    private System.Windows.Forms.TableLayoutPanel layoutButtons;
    private System.Windows.Forms.Button btnUpload;
    private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblFooter;
    }
}
