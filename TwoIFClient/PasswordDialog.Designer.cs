namespace TwoIFClient
{
    partial class PasswordDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            PromptLabel = new System.Windows.Forms.Label();
            PasswordBox = new System.Windows.Forms.TextBox();
            OkButton = new System.Windows.Forms.Button();
            CancelButton = new System.Windows.Forms.Button();
            SuspendLayout();

            // PromptLabel
            PromptLabel.Location = new System.Drawing.Point(12, 12);
            PromptLabel.Size = new System.Drawing.Size(360, 20);
            PromptLabel.Font = new System.Drawing.Font("Verdana", 9F);
            PromptLabel.ForeColor = System.Drawing.Color.Silver;

            // PasswordBox
            PasswordBox.Location = new System.Drawing.Point(12, 38);
            PasswordBox.Size = new System.Drawing.Size(360, 23);
            PasswordBox.PasswordChar = '●';
            PasswordBox.Font = new System.Drawing.Font("Verdana", 10F);
            PasswordBox.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            PasswordBox.ForeColor = System.Drawing.Color.Silver;
            PasswordBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            PasswordBox.KeyDown += PasswordBox_KeyDown;

            // OkButton
            OkButton.Text = "OK";
            OkButton.Location = new System.Drawing.Point(216, 74);
            OkButton.Size = new System.Drawing.Size(75, 28);
            OkButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            OkButton.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            OkButton.ForeColor = System.Drawing.Color.Silver;
            OkButton.Click += OkButton_Click;

            // CancelButton
            CancelButton.Text = "Cancel";
            CancelButton.Location = new System.Drawing.Point(297, 74);
            CancelButton.Size = new System.Drawing.Size(75, 28);
            CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CancelButton.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            CancelButton.ForeColor = System.Drawing.Color.Silver;
            CancelButton.Click += CancelButton_Click;

            // Form
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(384, 114);
            BackColor = System.Drawing.Color.FromArgb(24, 24, 24);
            Controls.AddRange(new System.Windows.Forms.Control[] { PromptLabel, PasswordBox, OkButton, CancelButton });
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Password";
            ResumeLayout(false);
        }

        private System.Windows.Forms.Label PromptLabel;
        private System.Windows.Forms.TextBox PasswordBox;
        private System.Windows.Forms.Button OkButton;
        private new System.Windows.Forms.Button CancelButton; // 'new' to shadow Form.CancelButton
    }
}