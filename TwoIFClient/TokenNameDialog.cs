using System;
using System.Windows.Forms;

namespace TwoIFClient
{
    /// <summary>
    /// Shown when an imported URI carries no issuer/label of its own.
    /// The user must supply a Name; Account is optional.
    /// </summary>
    public partial class TokenNameDialog : Form
    {
        public string TokenName => NameBox.Text.Trim();
        public string TokenAccount => AccountBox.Text.Trim();

        public TokenNameDialog(int backcolorIntensity = 24)
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(backcolorIntensity, backcolorIntensity, backcolorIntensity);
            this.OkButton.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.CancelButton.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.NameBox.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.AccountBox.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("A token name is required.", "Missing Name",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                NameBox.Focus();
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void NameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) OkButton_Click(sender, e);
            if (e.KeyCode == Keys.Escape) CancelButton_Click(sender, e);
        }
    }
}