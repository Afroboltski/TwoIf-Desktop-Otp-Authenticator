using System;
using System.Windows.Forms;

namespace TwoIFClient
{
    public partial class PasswordDialog : Form
    {
        public string Password => PasswordBox.Text;

        public PasswordDialog(string prompt = "Enter database password:")
        {
            InitializeComponent();
            PromptLabel.Text = prompt;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Text))
            {
                MessageBox.Show("Password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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