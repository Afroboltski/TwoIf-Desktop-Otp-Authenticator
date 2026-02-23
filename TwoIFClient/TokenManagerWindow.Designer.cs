namespace TwoIFClient
{
    partial class TokenManagerWindow
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            TokenListBox = new ListBox();
            SelectButton = new Button();
            DeleteButton = new Button();
            CloseButton = new Button();
            AddQrButton = new Button();
            AddManuallyButton = new Button();
            NameTextBox = new TextBox();
            SecretTextBox = new TextBox();
            NameLabel = new Label();
            SecretLabel = new Label();
            DividerLabel = new Label();
            SectionLabel = new Label();
            UriAddLabel = new Label();
            UriAddTextBox = new TextBox();
            AddUriButton = new Button();
            AccountTextBox = new TextBox();
            IssuerLabel = new Label();
            DigitsLabel = new Label();
            Opt6Digits = new RadioButton();
            Opt7Digits = new RadioButton();
            Opt8Digits = new RadioButton();
            AlgorithmLabel = new Label();
            OptSHA1 = new RadioButton();
            OptSHA256 = new RadioButton();
            OptSHA512 = new RadioButton();
            DigitsGroup = new GroupBox();
            groupBox1 = new GroupBox();
            TypeLabel = new Label();
            groupBox2 = new GroupBox();
            OptTOTP = new RadioButton();
            OptHOTP = new RadioButton();
            label3 = new Label();
            CounterOrPeriodLabel = new Label();
            CountOrPeriodTextBox = new TextBox();
            ChangePasswordButton = new Button();
            label1 = new Label();
            label2 = new Label();
            DigitsGroup.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // TokenListBox
            // 
            TokenListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TokenListBox.BackColor = Color.FromArgb(40, 40, 40);
            TokenListBox.BorderStyle = BorderStyle.FixedSingle;
            TokenListBox.Font = new Font("Verdana", 10F);
            TokenListBox.ForeColor = Color.Silver;
            TokenListBox.Location = new Point(12, 32);
            TokenListBox.Name = "TokenListBox";
            TokenListBox.Size = new Size(739, 130);
            TokenListBox.TabIndex = 0;
            TokenListBox.SelectedIndexChanged += TokenListBox_SelectedIndexChanged;
            TokenListBox.DoubleClick += TokenListBox_DoubleClick;
            // 
            // SelectButton
            // 
            SelectButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            SelectButton.BackColor = Color.FromArgb(40, 40, 40);
            SelectButton.Enabled = false;
            SelectButton.FlatStyle = FlatStyle.Flat;
            SelectButton.Font = new Font("Verdana", 9F);
            SelectButton.ForeColor = Color.Silver;
            SelectButton.Location = new Point(12, 172);
            SelectButton.Name = "SelectButton";
            SelectButton.Size = new Size(140, 28);
            SelectButton.TabIndex = 1;
            SelectButton.Text = "✔  Use Selected";
            SelectButton.UseVisualStyleBackColor = false;
            SelectButton.Click += SelectButton_Click;
            // 
            // DeleteButton
            // 
            DeleteButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            DeleteButton.BackColor = Color.FromArgb(40, 40, 40);
            DeleteButton.Enabled = false;
            DeleteButton.FlatStyle = FlatStyle.Flat;
            DeleteButton.Font = new Font("Verdana", 9F);
            DeleteButton.ForeColor = Color.FromArgb(220, 80, 80);
            DeleteButton.Location = new Point(160, 172);
            DeleteButton.Name = "DeleteButton";
            DeleteButton.Size = new Size(100, 28);
            DeleteButton.TabIndex = 2;
            DeleteButton.Text = "✖  Delete";
            DeleteButton.UseVisualStyleBackColor = false;
            DeleteButton.Click += DeleteButton_Click;
            // 
            // CloseButton
            // 
            CloseButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            CloseButton.BackColor = Color.FromArgb(40, 40, 40);
            CloseButton.FlatStyle = FlatStyle.Flat;
            CloseButton.Font = new Font("Verdana", 9F);
            CloseButton.ForeColor = Color.Silver;
            CloseButton.Location = new Point(591, 648);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(160, 28);
            CloseButton.TabIndex = 70;
            CloseButton.Text = "Close";
            CloseButton.UseVisualStyleBackColor = false;
            CloseButton.Click += CloseButton_Click;
            // 
            // AddQrButton
            // 
            AddQrButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            AddQrButton.BackColor = Color.FromArgb(40, 40, 40);
            AddQrButton.FlatStyle = FlatStyle.Flat;
            AddQrButton.Font = new Font("Verdana", 9F);
            AddQrButton.ForeColor = Color.Silver;
            AddQrButton.Location = new Point(12, 222);
            AddQrButton.Name = "AddQrButton";
            AddQrButton.Size = new Size(739, 28);
            AddQrButton.TabIndex = 10;
            AddQrButton.Text = "📷  Add from QR Image";
            AddQrButton.UseVisualStyleBackColor = false;
            AddQrButton.Click += AddQrButton_Click;
            // 
            // AddManuallyButton
            // 
            AddManuallyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            AddManuallyButton.BackColor = Color.FromArgb(40, 40, 40);
            AddManuallyButton.FlatStyle = FlatStyle.Flat;
            AddManuallyButton.Font = new Font("Verdana", 9F);
            AddManuallyButton.ForeColor = Color.Silver;
            AddManuallyButton.Location = new Point(12, 597);
            AddManuallyButton.Name = "AddManuallyButton";
            AddManuallyButton.Size = new Size(739, 28);
            AddManuallyButton.TabIndex = 60;
            AddManuallyButton.Text = "＋  Add Manually";
            AddManuallyButton.UseVisualStyleBackColor = false;
            AddManuallyButton.Click += AddManuallyButton_Click;
            // 
            // NameTextBox
            // 
            NameTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            NameTextBox.BackColor = Color.FromArgb(40, 40, 40);
            NameTextBox.BorderStyle = BorderStyle.FixedSingle;
            NameTextBox.Font = new Font("Verdana", 9F);
            NameTextBox.ForeColor = Color.Silver;
            NameTextBox.Location = new Point(189, 355);
            NameTextBox.Name = "NameTextBox";
            NameTextBox.Size = new Size(562, 22);
            NameTextBox.TabIndex = 30;
            // 
            // SecretTextBox
            // 
            SecretTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SecretTextBox.BackColor = Color.FromArgb(40, 40, 40);
            SecretTextBox.BorderStyle = BorderStyle.FixedSingle;
            SecretTextBox.Font = new Font("Verdana", 9F);
            SecretTextBox.ForeColor = Color.Silver;
            SecretTextBox.Location = new Point(189, 565);
            SecretTextBox.Name = "SecretTextBox";
            SecretTextBox.Size = new Size(562, 22);
            SecretTextBox.TabIndex = 50;
            // 
            // NameLabel
            // 
            NameLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            NameLabel.AutoSize = true;
            NameLabel.Font = new Font("Verdana", 9F);
            NameLabel.ForeColor = Color.Silver;
            NameLabel.Location = new Point(12, 358);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new Size(92, 14);
            NameLabel.TabIndex = 6;
            NameLabel.Text = "Issuer Name:";
            // 
            // SecretLabel
            // 
            SecretLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            SecretLabel.AutoSize = true;
            SecretLabel.Font = new Font("Verdana", 9F);
            SecretLabel.ForeColor = Color.Silver;
            SecretLabel.Location = new Point(12, 568);
            SecretLabel.Name = "SecretLabel";
            SecretLabel.Size = new Size(52, 14);
            SecretLabel.TabIndex = 8;
            SecretLabel.Text = "Secret:";
            // 
            // DividerLabel
            // 
            DividerLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            DividerLabel.BorderStyle = BorderStyle.Fixed3D;
            DividerLabel.Location = new Point(12, 210);
            DividerLabel.Name = "DividerLabel";
            DividerLabel.Size = new Size(739, 2);
            DividerLabel.TabIndex = 4;
            // 
            // SectionLabel
            // 
            SectionLabel.Font = new Font("Verdana", 9F, FontStyle.Bold);
            SectionLabel.ForeColor = Color.Silver;
            SectionLabel.Location = new Point(12, 10);
            SectionLabel.Name = "SectionLabel";
            SectionLabel.Size = new Size(200, 18);
            SectionLabel.TabIndex = 0;
            SectionLabel.Text = "Stored Tokens";
            // 
            // UriAddLabel
            // 
            UriAddLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            UriAddLabel.AutoSize = true;
            UriAddLabel.Font = new Font("Verdana", 9F);
            UriAddLabel.ForeColor = Color.Silver;
            UriAddLabel.Location = new Point(12, 275);
            UriAddLabel.Name = "UriAddLabel";
            UriAddLabel.Size = new Size(34, 14);
            UriAddLabel.TabIndex = 14;
            UriAddLabel.Text = "URI:";
            // 
            // UriAddTextBox
            // 
            UriAddTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            UriAddTextBox.BackColor = Color.FromArgb(40, 40, 40);
            UriAddTextBox.BorderStyle = BorderStyle.FixedSingle;
            UriAddTextBox.Font = new Font("Verdana", 9F);
            UriAddTextBox.ForeColor = Color.Silver;
            UriAddTextBox.Location = new Point(52, 272);
            UriAddTextBox.Name = "UriAddTextBox";
            UriAddTextBox.Size = new Size(699, 22);
            UriAddTextBox.TabIndex = 20;
            // 
            // AddUriButton
            // 
            AddUriButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            AddUriButton.BackColor = Color.FromArgb(40, 40, 40);
            AddUriButton.FlatStyle = FlatStyle.Flat;
            AddUriButton.Font = new Font("Verdana", 9F);
            AddUriButton.ForeColor = Color.Silver;
            AddUriButton.Location = new Point(12, 304);
            AddUriButton.Name = "AddUriButton";
            AddUriButton.Size = new Size(739, 28);
            AddUriButton.TabIndex = 21;
            AddUriButton.Text = "🔗  Add from URI";
            AddUriButton.UseVisualStyleBackColor = false;
            AddUriButton.Click += AddUriButton_Click;
            // 
            // AccountTextBox
            // 
            AccountTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            AccountTextBox.BackColor = Color.FromArgb(40, 40, 40);
            AccountTextBox.BorderStyle = BorderStyle.FixedSingle;
            AccountTextBox.Font = new Font("Verdana", 9F);
            AccountTextBox.ForeColor = Color.Silver;
            AccountTextBox.Location = new Point(189, 390);
            AccountTextBox.Name = "AccountTextBox";
            AccountTextBox.Size = new Size(562, 22);
            AccountTextBox.TabIndex = 31;
            // 
            // IssuerLabel
            // 
            IssuerLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            IssuerLabel.AutoSize = true;
            IssuerLabel.Font = new Font("Verdana", 9F);
            IssuerLabel.ForeColor = Color.Silver;
            IssuerLabel.Location = new Point(12, 393);
            IssuerLabel.Name = "IssuerLabel";
            IssuerLabel.Size = new Size(168, 14);
            IssuerLabel.TabIndex = 6;
            IssuerLabel.Text = "Account Name (Optional):";
            // 
            // DigitsLabel
            // 
            DigitsLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            DigitsLabel.AutoSize = true;
            DigitsLabel.Font = new Font("Verdana", 9F);
            DigitsLabel.ForeColor = Color.Silver;
            DigitsLabel.Location = new Point(12, 498);
            DigitsLabel.Name = "DigitsLabel";
            DigitsLabel.Size = new Size(116, 14);
            DigitsLabel.TabIndex = 6;
            DigitsLabel.Text = "Number of Digits:";
            // 
            // Opt6Digits
            // 
            Opt6Digits.AutoSize = true;
            Opt6Digits.Checked = true;
            Opt6Digits.ForeColor = Color.Silver;
            Opt6Digits.Location = new Point(17, 12);
            Opt6Digits.Name = "Opt6Digits";
            Opt6Digits.Size = new Size(31, 19);
            Opt6Digits.TabIndex = 35;
            Opt6Digits.TabStop = true;
            Opt6Digits.Text = "6";
            Opt6Digits.UseVisualStyleBackColor = true;
            // 
            // Opt7Digits
            // 
            Opt7Digits.AutoSize = true;
            Opt7Digits.ForeColor = Color.Silver;
            Opt7Digits.Location = new Point(98, 12);
            Opt7Digits.Name = "Opt7Digits";
            Opt7Digits.Size = new Size(31, 19);
            Opt7Digits.TabIndex = 36;
            Opt7Digits.Text = "7";
            Opt7Digits.UseVisualStyleBackColor = true;
            // 
            // Opt8Digits
            // 
            Opt8Digits.AutoSize = true;
            Opt8Digits.ForeColor = Color.Silver;
            Opt8Digits.Location = new Point(179, 12);
            Opt8Digits.Name = "Opt8Digits";
            Opt8Digits.Size = new Size(31, 19);
            Opt8Digits.TabIndex = 37;
            Opt8Digits.Text = "8";
            Opt8Digits.UseVisualStyleBackColor = true;
            // 
            // AlgorithmLabel
            // 
            AlgorithmLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            AlgorithmLabel.AutoSize = true;
            AlgorithmLabel.Font = new Font("Verdana", 9F);
            AlgorithmLabel.ForeColor = Color.Silver;
            AlgorithmLabel.Location = new Point(12, 533);
            AlgorithmLabel.Name = "AlgorithmLabel";
            AlgorithmLabel.Size = new Size(71, 14);
            AlgorithmLabel.TabIndex = 6;
            AlgorithmLabel.Text = "Algorithm:";
            // 
            // OptSHA1
            // 
            OptSHA1.AutoSize = true;
            OptSHA1.Checked = true;
            OptSHA1.ForeColor = Color.Silver;
            OptSHA1.Location = new Point(17, 12);
            OptSHA1.Name = "OptSHA1";
            OptSHA1.Size = new Size(54, 19);
            OptSHA1.TabIndex = 38;
            OptSHA1.TabStop = true;
            OptSHA1.Text = "SHA1";
            OptSHA1.UseVisualStyleBackColor = true;
            // 
            // OptSHA256
            // 
            OptSHA256.AutoSize = true;
            OptSHA256.ForeColor = Color.Silver;
            OptSHA256.Location = new Point(98, 12);
            OptSHA256.Name = "OptSHA256";
            OptSHA256.Size = new Size(66, 19);
            OptSHA256.TabIndex = 39;
            OptSHA256.Text = "SHA256";
            OptSHA256.UseVisualStyleBackColor = true;
            // 
            // OptSHA512
            // 
            OptSHA512.AutoSize = true;
            OptSHA512.ForeColor = Color.Silver;
            OptSHA512.Location = new Point(179, 12);
            OptSHA512.Name = "OptSHA512";
            OptSHA512.Size = new Size(66, 19);
            OptSHA512.TabIndex = 40;
            OptSHA512.Text = "SHA512";
            OptSHA512.UseVisualStyleBackColor = true;
            // 
            // DigitsGroup
            // 
            DigitsGroup.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            DigitsGroup.Controls.Add(Opt8Digits);
            DigitsGroup.Controls.Add(Opt6Digits);
            DigitsGroup.Controls.Add(Opt7Digits);
            DigitsGroup.ForeColor = Color.Silver;
            DigitsGroup.Location = new Point(189, 484);
            DigitsGroup.Name = "DigitsGroup";
            DigitsGroup.Size = new Size(562, 36);
            DigitsGroup.TabIndex = 61;
            DigitsGroup.TabStop = false;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(OptSHA1);
            groupBox1.Controls.Add(OptSHA256);
            groupBox1.Controls.Add(OptSHA512);
            groupBox1.ForeColor = Color.Silver;
            groupBox1.Location = new Point(189, 519);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(562, 36);
            groupBox1.TabIndex = 62;
            groupBox1.TabStop = false;
            // 
            // TypeLabel
            // 
            TypeLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            TypeLabel.AutoSize = true;
            TypeLabel.Font = new Font("Verdana", 9F);
            TypeLabel.ForeColor = Color.Silver;
            TypeLabel.Location = new Point(12, 428);
            TypeLabel.Name = "TypeLabel";
            TypeLabel.Size = new Size(65, 14);
            TypeLabel.TabIndex = 6;
            TypeLabel.Text = "OPT Type";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(OptTOTP);
            groupBox2.Controls.Add(OptHOTP);
            groupBox2.ForeColor = Color.Silver;
            groupBox2.Location = new Point(189, 414);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(562, 36);
            groupBox2.TabIndex = 61;
            groupBox2.TabStop = false;
            // 
            // OptTOTP
            // 
            OptTOTP.AutoSize = true;
            OptTOTP.Checked = true;
            OptTOTP.ForeColor = Color.Silver;
            OptTOTP.Location = new Point(17, 12);
            OptTOTP.Name = "OptTOTP";
            OptTOTP.Size = new Size(113, 19);
            OptTOTP.TabIndex = 32;
            OptTOTP.TabStop = true;
            OptTOTP.Text = "Time-Based OTP";
            OptTOTP.UseVisualStyleBackColor = true;
            OptTOTP.CheckedChanged += OptTOTP_CheckedChanged;
            // 
            // OptHOTP
            // 
            OptHOTP.AutoSize = true;
            OptHOTP.ForeColor = Color.Silver;
            OptHOTP.Location = new Point(179, 12);
            OptHOTP.Name = "OptHOTP";
            OptHOTP.Size = new Size(176, 19);
            OptHOTP.TabIndex = 33;
            OptHOTP.Text = "HMAC-Based (Counter) OTP";
            OptHOTP.UseVisualStyleBackColor = true;
            OptHOTP.CheckedChanged += OptHOTP_CheckedChanged;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label3.BorderStyle = BorderStyle.Fixed3D;
            label3.Location = new Point(12, 635);
            label3.Name = "label3";
            label3.Size = new Size(739, 2);
            label3.TabIndex = 13;
            // 
            // CounterOrPeriodLabel
            // 
            CounterOrPeriodLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            CounterOrPeriodLabel.AutoSize = true;
            CounterOrPeriodLabel.Font = new Font("Verdana", 9F);
            CounterOrPeriodLabel.ForeColor = Color.Silver;
            CounterOrPeriodLabel.Location = new Point(12, 463);
            CounterOrPeriodLabel.Name = "CounterOrPeriodLabel";
            CounterOrPeriodLabel.Size = new Size(73, 14);
            CounterOrPeriodLabel.TabIndex = 71;
            CounterOrPeriodLabel.Text = "Period (s):";
            // 
            // CountOrPeriodTextBox
            // 
            CountOrPeriodTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            CountOrPeriodTextBox.BackColor = Color.FromArgb(40, 40, 40);
            CountOrPeriodTextBox.BorderStyle = BorderStyle.FixedSingle;
            CountOrPeriodTextBox.Font = new Font("Verdana", 9F);
            CountOrPeriodTextBox.ForeColor = Color.Silver;
            CountOrPeriodTextBox.Location = new Point(189, 460);
            CountOrPeriodTextBox.Name = "CountOrPeriodTextBox";
            CountOrPeriodTextBox.Size = new Size(562, 22);
            CountOrPeriodTextBox.TabIndex = 34;
            CountOrPeriodTextBox.Text = "30";
            // 
            // ChangePasswordButton
            // 
            ChangePasswordButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChangePasswordButton.BackColor = Color.FromArgb(40, 40, 40);
            ChangePasswordButton.FlatStyle = FlatStyle.Flat;
            ChangePasswordButton.Font = new Font("Verdana", 9F);
            ChangePasswordButton.ForeColor = Color.FromArgb(180, 180, 100);
            ChangePasswordButton.Location = new Point(12, 648);
            ChangePasswordButton.Name = "ChangePasswordButton";
            ChangePasswordButton.Size = new Size(200, 28);
            ChangePasswordButton.TabIndex = 71;
            ChangePasswordButton.Text = "🔑  Change Password…";
            ChangePasswordButton.UseVisualStyleBackColor = false;
            ChangePasswordButton.Click += ChangePasswordButton_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.BorderStyle = BorderStyle.Fixed3D;
            label1.Location = new Point(12, 343);
            label1.Name = "label1";
            label1.Size = new Size(739, 2);
            label1.TabIndex = 72;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.BorderStyle = BorderStyle.Fixed3D;
            label2.Location = new Point(12, 260);
            label2.Name = "label2";
            label2.Size = new Size(739, 2);
            label2.TabIndex = 73;
            // 
            // TokenManagerWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 24, 24);
            ClientSize = new Size(763, 690);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(CounterOrPeriodLabel);
            Controls.Add(CountOrPeriodTextBox);
            Controls.Add(groupBox1);
            Controls.Add(groupBox2);
            Controls.Add(DigitsGroup);
            Controls.Add(AddUriButton);
            Controls.Add(UriAddLabel);
            Controls.Add(UriAddTextBox);
            Controls.Add(label3);
            Controls.Add(SectionLabel);
            Controls.Add(TokenListBox);
            Controls.Add(SelectButton);
            Controls.Add(DeleteButton);
            Controls.Add(DividerLabel);
            Controls.Add(AddQrButton);
            Controls.Add(AlgorithmLabel);
            Controls.Add(TypeLabel);
            Controls.Add(DigitsLabel);
            Controls.Add(IssuerLabel);
            Controls.Add(AccountTextBox);
            Controls.Add(NameLabel);
            Controls.Add(NameTextBox);
            Controls.Add(SecretLabel);
            Controls.Add(SecretTextBox);
            Controls.Add(AddManuallyButton);
            Controls.Add(ChangePasswordButton);
            Controls.Add(CloseButton);
            DoubleBuffered = true;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(612, 630);
            Name = "TokenManagerWindow";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Manage Tokens";
            Load += TokenManagerWindow_Load;
            DigitsGroup.ResumeLayout(false);
            DigitsGroup.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ListBox  TokenListBox;
        private System.Windows.Forms.Button   SelectButton;
        private System.Windows.Forms.Button   DeleteButton;
        private System.Windows.Forms.Button   CloseButton;
        private System.Windows.Forms.Button   AddQrButton;
        private System.Windows.Forms.Button   AddManuallyButton;
        private System.Windows.Forms.TextBox  NameTextBox;
        private System.Windows.Forms.TextBox  SecretTextBox;
        private System.Windows.Forms.Label    NameLabel;
        private System.Windows.Forms.Label    SecretLabel;
        private System.Windows.Forms.Label    DividerLabel;
        private System.Windows.Forms.Label    SectionLabel;
        private Label UriAddLabel;
        private TextBox UriAddTextBox;
        private Button AddUriButton;
        private TextBox AccountTextBox;
        private Label IssuerLabel;
        private Label DigitsLabel;
        private RadioButton Opt6Digits;
        private RadioButton Opt7Digits;
        private RadioButton Opt8Digits;
        private Label AlgorithmLabel;
        private RadioButton OptSHA1;
        private RadioButton OptSHA256;
        private RadioButton OptSHA512;
        private GroupBox DigitsGroup;
        private GroupBox groupBox1;
        private Label TypeLabel;
        private GroupBox groupBox2;
        private RadioButton OptTOTP;
        private RadioButton OptHOTP;
        private Label label3;
        private Label CounterOrPeriodLabel;
        private TextBox CountOrPeriodTextBox;
        private Button ChangePasswordButton;
        private Label label1;
        private Label label2;
    }
}