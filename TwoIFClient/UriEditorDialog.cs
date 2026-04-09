using System;
using System.Windows.Forms;

namespace TwoIFClient
{
    public partial class UriEditorDialog : Form
    {
        public string Uri => UriBox.Text;

        public UriEditorDialog(string prompt = "Token URI Review", string prepopulatedText = null, int backcolorIntensity = 24)
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(backcolorIntensity, backcolorIntensity, backcolorIntensity);
            this.CancelButton.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.OkButton.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.UriBox.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            PromptLabel.Text = prompt;
            if(prepopulatedText!=null)
            {
                this.UriBox.Text = prepopulatedText;
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UriBox.Text))
            {
                MessageBox.Show("Token URI cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UriBox.Focus();
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

        private void UriBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) OkButton_Click(sender, e);
            if (e.KeyCode == Keys.Escape) CancelButton_Click(sender, e);
        }
    }
}