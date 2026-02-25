namespace TwoIFClient
{
    partial class TwoIFClientWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        private System.Windows.Forms.Label CodeLabel;

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
            components = new System.ComponentModel.Container();
            CodeLabel = new Label();
            FlashTimer = new System.Windows.Forms.Timer(components);
            RefreshTimer = new System.Windows.Forms.Timer(components);
            TimeLabel = new Label();
            Hamburger = new Label();
            NameLabel = new Label();
            CountEntry = new NumericUpDown();
            CountLabel = new Label();
            AccountLabel = new Label();
            ApplyCounterTimer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)CountEntry).BeginInit();
            SuspendLayout();
            // 
            // CodeLabel
            // 
            CodeLabel.BackColor = Color.Black;
            CodeLabel.Cursor = Cursors.Hand;
            CodeLabel.Dock = DockStyle.Fill;
            CodeLabel.Font = new Font("Verdana", 48F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CodeLabel.ForeColor = Color.Silver;
            CodeLabel.Location = new Point(0, 0);
            CodeLabel.Name = "CodeLabel";
            CodeLabel.Size = new Size(418, 171);
            CodeLabel.TabIndex = 0;
            CodeLabel.Text = "000000";
            CodeLabel.TextAlign = ContentAlignment.MiddleCenter;
            CodeLabel.Click += CodeLabel_Click;
            // 
            // FlashTimer
            // 
            FlashTimer.Interval = 10;
            FlashTimer.Tick += FlashTimer_Tick;
            // 
            // RefreshTimer
            // 
            RefreshTimer.Enabled = true;
            RefreshTimer.Interval = 500;
            RefreshTimer.Tick += RefreshTimer_Tick;
            // 
            // TimeLabel
            // 
            TimeLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            TimeLabel.BackColor = Color.Black;
            TimeLabel.Cursor = Cursors.Hand;
            TimeLabel.Font = new Font("Verdana", 12F, FontStyle.Bold);
            TimeLabel.ForeColor = Color.Silver;
            TimeLabel.Location = new Point(340, 135);
            TimeLabel.Name = "TimeLabel";
            TimeLabel.Size = new Size(66, 23);
            TimeLabel.TabIndex = 1;
            TimeLabel.Text = "30";
            TimeLabel.TextAlign = ContentAlignment.TopRight;
            TimeLabel.Click += TimeLabel_Click;
            // 
            // Hamburger
            // 
            Hamburger.AutoSize = true;
            Hamburger.BackColor = Color.Black;
            Hamburger.Cursor = Cursors.Hand;
            Hamburger.Font = new Font("Verdana", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Hamburger.ForeColor = Color.Silver;
            Hamburger.Location = new Point(10, 7);
            Hamburger.Name = "Hamburger";
            Hamburger.Size = new Size(36, 32);
            Hamburger.TabIndex = 2;
            Hamburger.Text = "☰";
            Hamburger.Click += Hamburger_Click;
            Hamburger.MouseEnter += Hamburger_MouseEnter;
            Hamburger.MouseLeave += Hamburger_MouseLeave;
            // 
            // NameLabel
            // 
            NameLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            NameLabel.BackColor = Color.Black;
            NameLabel.Cursor = Cursors.Hand;
            NameLabel.Font = new Font("Verdana", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            NameLabel.ForeColor = Color.Silver;
            NameLabel.Location = new Point(12, 11);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new Size(394, 23);
            NameLabel.TabIndex = 3;
            NameLabel.Text = "Name";
            NameLabel.TextAlign = ContentAlignment.MiddleCenter;
            NameLabel.Click += NameLabel_Click;
            // 
            // CountEntry
            // 
            CountEntry.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            CountEntry.BackColor = Color.Black;
            CountEntry.BorderStyle = BorderStyle.FixedSingle;
            CountEntry.Font = new Font("Verdana", 12F, FontStyle.Bold);
            CountEntry.ForeColor = Color.Silver;
            CountEntry.Location = new Point(152, 133);
            CountEntry.Maximum = new decimal(new int[] { -1, int.MaxValue, 0, 0 });
            CountEntry.Name = "CountEntry";
            CountEntry.Size = new Size(213, 27);
            CountEntry.TabIndex = 4;
            CountEntry.Visible = false;
            CountEntry.ValueChanged += CountEntry_ValueChanged;
            CountEntry.KeyDown += CountEntry_KeyDown;
            CountEntry.Leave += CountEntry_Leave;
            // 
            // CountLabel
            // 
            CountLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            CountLabel.AutoSize = true;
            CountLabel.BackColor = Color.Black;
            CountLabel.Cursor = Cursors.Hand;
            CountLabel.Font = new Font("Verdana", 12F, FontStyle.Bold);
            CountLabel.ForeColor = Color.Silver;
            CountLabel.Location = new Point(42, 135);
            CountLabel.Name = "CountLabel";
            CountLabel.Size = new Size(85, 18);
            CountLabel.TabIndex = 5;
            CountLabel.Text = "Counter:";
            CountLabel.TextAlign = ContentAlignment.MiddleLeft;
            CountLabel.Visible = false;
            CountLabel.Click += CountLabel_Click;
            // 
            // AccountLabel
            // 
            AccountLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            AccountLabel.BackColor = Color.Black;
            AccountLabel.Cursor = Cursors.Hand;
            AccountLabel.Font = new Font("Verdana", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            AccountLabel.ForeColor = Color.Silver;
            AccountLabel.Location = new Point(12, 34);
            AccountLabel.Name = "AccountLabel";
            AccountLabel.Size = new Size(394, 22);
            AccountLabel.TabIndex = 6;
            AccountLabel.Text = "Account Name";
            AccountLabel.TextAlign = ContentAlignment.TopCenter;
            AccountLabel.Click += AccountLabel_Click;
            // 
            // ApplyCounterTimer
            // 
            ApplyCounterTimer.Interval = 500;
            ApplyCounterTimer.Tick += ApplyCounterTimer_Tick;
            // 
            // TwoIFClientWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(418, 171);
            Controls.Add(Hamburger);
            Controls.Add(AccountLabel);
            Controls.Add(NameLabel);
            Controls.Add(CountEntry);
            Controls.Add(CountLabel);
            Controls.Add(TimeLabel);
            Controls.Add(CodeLabel);
            DoubleBuffered = true;
            MaximizeBox = false;
            MaximumSize = new Size(1066, 600);
            MinimizeBox = false;
            MinimumSize = new Size(434, 210);
            Name = "TwoIFClientWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TwoIF One-Time Code";
            Load += TwoIFClientWindow_Load;
            ((System.ComponentModel.ISupportInitialize)CountEntry).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer FlashTimer;
        private System.Windows.Forms.Timer RefreshTimer;
        private Label TimeLabel;
        private Label Hamburger;
        private Label NameLabel;
        private NumericUpDown CountEntry;
        private Label CountLabel;
        private Label AccountLabel;
        private System.Windows.Forms.Timer ApplyCounterTimer;
    }
}
