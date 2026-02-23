namespace TwoIFClient
{
    partial class TokenNameDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            IntroLabel = new Label();
            NameLabel = new Label();
            NameBox = new TextBox();
            AccountLabel = new Label();
            AccountBox = new TextBox();
            OkButton = new Button();
            CancelButton = new Button();
            SuspendLayout();
            // 
            // IntroLabel
            // 
            IntroLabel.Font = new Font("Verdana", 9F, FontStyle.Italic);
            IntroLabel.ForeColor = Color.FromArgb(200, 180, 100);
            IntroLabel.Location = new Point(12, 10);
            IntroLabel.Name = "IntroLabel";
            IntroLabel.Size = new Size(430, 28);
            IntroLabel.TabIndex = 0;
            IntroLabel.Text = "This token has no name. Please provide one before saving.";
            // 
            // NameLabel
            // 
            NameLabel.Font = new Font("Verdana", 9F);
            NameLabel.ForeColor = Color.Silver;
            NameLabel.Location = new Point(12, 48);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new Size(80, 18);
            NameLabel.TabIndex = 1;
            NameLabel.Text = "Name:";
            // 
            // NameBox
            // 
            NameBox.BackColor = Color.FromArgb(40, 40, 40);
            NameBox.BorderStyle = BorderStyle.FixedSingle;
            NameBox.Font = new Font("Verdana", 9F);
            NameBox.ForeColor = Color.Silver;
            NameBox.Location = new Point(146, 45);
            NameBox.Name = "NameBox";
            NameBox.Size = new Size(296, 22);
            NameBox.TabIndex = 2;
            NameBox.KeyDown += NameBox_KeyDown;
            // 
            // AccountLabel
            // 
            AccountLabel.Font = new Font("Verdana", 9F);
            AccountLabel.ForeColor = Color.Silver;
            AccountLabel.Location = new Point(12, 78);
            AccountLabel.Name = "AccountLabel";
            AccountLabel.Size = new Size(130, 18);
            AccountLabel.TabIndex = 3;
            AccountLabel.Text = "Account (Optional):";
            // 
            // AccountBox
            // 
            AccountBox.BackColor = Color.FromArgb(40, 40, 40);
            AccountBox.BorderStyle = BorderStyle.FixedSingle;
            AccountBox.Font = new Font("Verdana", 9F);
            AccountBox.ForeColor = Color.Silver;
            AccountBox.Location = new Point(146, 75);
            AccountBox.Name = "AccountBox";
            AccountBox.Size = new Size(296, 22);
            AccountBox.TabIndex = 4;
            // 
            // OkButton
            // 
            OkButton.BackColor = Color.FromArgb(40, 40, 40);
            OkButton.FlatStyle = FlatStyle.Flat;
            OkButton.Font = new Font("Verdana", 9F);
            OkButton.ForeColor = Color.Silver;
            OkButton.Location = new Point(282, 110);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(75, 28);
            OkButton.TabIndex = 5;
            OkButton.Text = "OK";
            OkButton.UseVisualStyleBackColor = false;
            OkButton.Click += OkButton_Click;
            // 
            // CancelButton
            // 
            CancelButton.BackColor = Color.FromArgb(40, 40, 40);
            CancelButton.FlatStyle = FlatStyle.Flat;
            CancelButton.Font = new Font("Verdana", 9F);
            CancelButton.ForeColor = Color.Silver;
            CancelButton.Location = new Point(367, 110);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(75, 28);
            CancelButton.TabIndex = 6;
            CancelButton.Text = "Cancel";
            CancelButton.UseVisualStyleBackColor = false;
            CancelButton.Click += CancelButton_Click;
            // 
            // TokenNameDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 24, 24);
            ClientSize = new Size(454, 150);
            Controls.Add(IntroLabel);
            Controls.Add(NameLabel);
            Controls.Add(NameBox);
            Controls.Add(AccountLabel);
            Controls.Add(AccountBox);
            Controls.Add(OkButton);
            Controls.Add(CancelButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TokenNameDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Name This Token";
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label IntroLabel;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Label AccountLabel;
        private System.Windows.Forms.TextBox AccountBox;
        private System.Windows.Forms.Button OkButton;
        private new System.Windows.Forms.Button CancelButton;
    }
}