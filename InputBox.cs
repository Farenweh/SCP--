using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScpLauncher
{
    internal class InputBox : Form
    {
        private TextBox txt;
        private Button btnOk;
        private Button btnCancel;
        public string Value { get; private set; }

    public InputBox(string prompt, string title = "输入", string initial = null)
        {
            this.Text = title;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ClientSize = new Size(420, 140);

            var lbl = new Label
            {
                Text = prompt,
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 36,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 8, 12, 4)
            };
            txt = new TextBox { Dock = DockStyle.Top, Margin = new Padding(12), Height = 28 };
            if (!string.IsNullOrEmpty(initial)) txt.Text = initial;
            var panelButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Height = 48,
                Padding = new Padding(0, 8, 12, 8)
            };
            btnOk = new Button { Text = "确定", DialogResult = DialogResult.OK, Width = 80, Height = 28, Margin = new Padding(8, 8, 0, 8) };
            btnCancel = new Button { Text = "取消", DialogResult = DialogResult.Cancel, Width = 80, Height = 28, Margin = new Padding(8, 8, 0, 8) };
            panelButtons.Controls.Add(btnOk);
            panelButtons.Controls.Add(btnCancel);

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;

            this.Controls.Add(panelButtons);
            this.Controls.Add(txt);
            this.Controls.Add(lbl);

            btnOk.Click += (s, e) => { Value = txt.Text; };
        }
    }
}
