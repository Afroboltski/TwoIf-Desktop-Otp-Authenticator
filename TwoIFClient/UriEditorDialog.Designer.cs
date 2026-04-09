namespace TwoIFClient
{
    partial class UriEditorDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            PromptLabel = new Label();
            UriBox = new TextBox();
            OkButton = new Button();
            CancelButton = new Button();
            SuspendLayout();
            // 
            // PromptLabel
            // 
            PromptLabel.Font = new Font("Verdana", 9F);
            PromptLabel.ForeColor = Color.Silver;
            PromptLabel.Location = new Point(12, 12);
            PromptLabel.Name = "PromptLabel";
            PromptLabel.Size = new Size(360, 20);
            PromptLabel.TabIndex = 0;
            // 
            // UriBox
            // 
            UriBox.BackColor = Color.FromArgb(40, 40, 40);
            UriBox.BorderStyle = BorderStyle.FixedSingle;
            UriBox.Font = new Font("Verdana", 10F);
            UriBox.ForeColor = Color.Silver;
            UriBox.Location = new Point(12, 38);
            UriBox.Name = "UriBox";
            UriBox.Size = new Size(360, 24);
            UriBox.TabIndex = 1;
            UriBox.KeyDown += UriBox_KeyDown;
            // 
            // OkButton
            // 
            OkButton.BackColor = Color.FromArgb(40, 40, 40);
            OkButton.FlatStyle = FlatStyle.Flat;
            OkButton.ForeColor = Color.Silver;
            OkButton.Location = new Point(216, 74);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(75, 28);
            OkButton.TabIndex = 2;
            OkButton.Text = "OK";
            OkButton.UseVisualStyleBackColor = false;
            OkButton.Click += OkButton_Click;
            // 
            // CancelButton
            // 
            CancelButton.BackColor = Color.FromArgb(40, 40, 40);
            CancelButton.FlatStyle = FlatStyle.Flat;
            CancelButton.ForeColor = Color.Silver;
            CancelButton.Location = new Point(297, 74);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(75, 28);
            CancelButton.TabIndex = 3;
            CancelButton.Text = "Cancel";
            CancelButton.UseVisualStyleBackColor = false;
            CancelButton.Click += CancelButton_Click;
            // 
            // UriEditorDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 24, 24);
            ClientSize = new Size(384, 114);
            Controls.Add(PromptLabel);
            Controls.Add(UriBox);
            Controls.Add(OkButton);
            Controls.Add(CancelButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "UriEditorDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "OTP URI Editor";
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label PromptLabel;
        private System.Windows.Forms.TextBox UriBox;
        private System.Windows.Forms.Button OkButton;
        private new System.Windows.Forms.Button CancelButton; // 'new' to shadow Form.CancelButton
    }
}