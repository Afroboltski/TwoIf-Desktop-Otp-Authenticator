using System;
using System.Windows.Forms;

namespace TwoIFClient
{
    public partial class PasswordDialog : Form
    {
        public string Password => PasswordBox.Text;

        public PasswordDialog(string prompt = "Enter database password:", int backcolorIntensity = 24)
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(backcolorIntensity, backcolorIntensity, backcolorIntensity);
            this.CancelButton.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.OkButton.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.PasswordBox.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            PromptLabel.Text = prompt;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Text))
            {
                MessageBox.Show("Password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PasswordBox.Focus();
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

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) OkButton_Click(sender, e);
            if (e.KeyCode == Keys.Escape) CancelButton_Click(sender, e);
        }
    }
}