using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using OtpLibrary;
using OtpNet;
using QRCodeLibrary;
using ZXing;

namespace TwoIFClient
{
    public partial class TokenManagerWindow : Form
    {
        private readonly OtpDatabase _database;

        /// <summary>
        /// The article the user has marked as active inside this window.
        /// The main window reads this on close.
        /// </summary>
        public OtpArticle SelectedArticle { get; private set; }

        public TokenManagerWindow(OtpDatabase database, OtpArticle currentArticle)
        {
            InitializeComponent();
            _database = database;
            SelectedArticle = currentArticle;
            PopulateList();
        }

        private void PopulateList()
        {
            int previousListSelection = TokenListBox.SelectedIndex;

            TokenListBox.Items.Clear();
            for (int i = 0; i < _database.OtpArticles.Count; i++)
            {
                var article = _database.OtpArticles[i];
                bool isActive = (article == SelectedArticle);
                // Green check for the active token, blank indent for others
                TokenListBox.Items.Add((isActive ? "✔ " : "    ") + article.Name);
            }

            // Restore the list highlight to where it was (or 0 if first load)
            if (TokenListBox.Items.Count > 0)
            {
                int restore = previousListSelection >= 0 && previousListSelection < TokenListBox.Items.Count
                    ? previousListSelection : 0;
                TokenListBox.SelectedIndex = restore;
            }

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = TokenListBox.SelectedIndex >= 0;
            SelectButton.Enabled = hasSelection;
            DeleteButton.Enabled = hasSelection;
        }

        private OtpArticle ArticleAtIndex(int listIndex) =>
            listIndex >= 0 && listIndex < _database.OtpArticles.Count
                ? _database.OtpArticles[listIndex]
                : null;

        private OtpArticle CurrentlyHighlightedArticle =>
            ArticleAtIndex(TokenListBox.SelectedIndex);

        private void AddQrButton_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*",
                Title = "Select a QR Code Image"
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            QRCode code;
            try { code = QRCode.LoadFromImage(dlg.FileName); }
            catch (Exception ex)
            {
                MessageBox.Show("Could not read QR code." + Environment.NewLine + Environment.NewLine + "" + ex.Message,
                    "QR Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TryAddArticleFromUri(code.Data);
        }

        // -- Add via secret string -------------------------------------------------

        private void AddManuallyButton_Click(object sender, EventArgs e)
        {
            string name = NameTextBox.Text.Trim();
            string account = AccountTextBox.Text.Trim();
            string secret = SecretTextBox.Text.Trim();

            OtpHashMode hashMode = OtpHashMode.Sha1;
            if (OptSHA1.Checked) { hashMode = OtpHashMode.Sha1; }
            else if (OptSHA256.Checked) { hashMode = OtpHashMode.Sha256; }
            else if (OptSHA512.Checked) { hashMode = OtpHashMode.Sha512; }
            string hashModeString = hashMode switch
            {
                OtpHashMode.Sha1 => "SHA1",
                OtpHashMode.Sha256 => "SHA256",
                OtpHashMode.Sha512 => "SHA512",
                _ => string.Empty
            };

            OtpType otpType = OtpType.Totp;
            if (OptTOTP.Checked) { otpType = OtpType.Totp; }
            else if (OptHOTP.Checked) { otpType = OtpType.Hotp; }
            string otpTypeString = otpType switch
            {
                OtpType.Totp => "totp",
                OtpType.Hotp => "hotp",
                _ => string.Empty
            };

            int counterOrPeriod = 0;
            string counterOrPeriodString = CountOrPeriodTextBox.Text.Trim();


            int digits = 6;
            if (Opt6Digits.Checked) { digits = 6; }
            if (Opt7Digits.Checked) { digits = 7; }
            if (Opt8Digits.Checked) { digits = 8; }
            string digitsString = digits.ToString();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a name for this token.", "Missing Name",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                NameTextBox.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(secret))
            {
                MessageBox.Show("Please enter the secret key.", "Missing Secret",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SecretTextBox.Focus();
                return;
            }

            if (!int.TryParse(counterOrPeriodString, out counterOrPeriod))
            {
                MessageBox.Show("Invalid counter/period value.", "Invalid Value",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CountOrPeriodTextBox.Focus();
                return;
            }

            string counterOrPeriodParamName = otpType switch
            {
                OtpType.Totp => "period",
                OtpType.Hotp => "counter",
                _ => string.Empty
            };

            // Build a URI to pass down the chain
            string uri = string.Format("otpauth://{0}/{1}{2}?secret={3}{4}{5}{6}",
                otpTypeString,
                name,
                string.IsNullOrWhiteSpace(account) ? "" : (":" + account),
                secret,
                string.IsNullOrWhiteSpace(hashModeString) ? "" : ("&algorithm=" + hashModeString),
                string.IsNullOrWhiteSpace(digitsString) ? "" : ("&digits=" + digitsString),
                (string.IsNullOrWhiteSpace(counterOrPeriodString) || string.IsNullOrWhiteSpace(counterOrPeriodParamName)) ? "" : ("&" + counterOrPeriodParamName + "=" + counterOrPeriodString)
            );

            if (TryAddArticleFromUri(uri))
            {
                NameTextBox.Clear();
                AccountTextBox.Clear();
                OptTOTP.Checked = true;
                OptSHA1.Checked = true;
                Opt6Digits.Checked = true;
                SecretTextBox.Clear();
                NameTextBox.Focus();
            }
        }

        private bool TryAddArticleFromUri(string uri)
        {
            OtpArticle article;
            try { article = new OtpArticle(uri); }
            catch (Exception ex)
            {
                MessageBox.Show("Could not parse token data." + Environment.NewLine + Environment.NewLine + "" + ex.Message,
                    "Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            _database.OtpArticles.Add(article);

            // Highlight the newly added row
            int newIdx = _database.OtpArticles.Count - 1;
            SelectedArticle = article;
            PopulateList();
            TokenListBox.SelectedIndex = newIdx;
            return true;
        }

        // -- Use Selected ----------------------------------------------------------

        private void SelectButton_Click(object sender, EventArgs e)
        {
            var article = CurrentlyHighlightedArticle;
            if (article == null) return;

            SelectedArticle = article;
            // Refresh list so the check mark moves to the new active token
            PopulateList();
        }

        // -- Delete ----------------------------------------------------------------

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var article = CurrentlyHighlightedArticle;
            if (article == null) return;

            var res = MessageBox.Show(
                $"Delete token \"{article.Name}\"? This cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res != DialogResult.Yes) return;

            if (article == SelectedArticle)
                SelectedArticle = null; // active token is gone

            int removedIdx = TokenListBox.SelectedIndex;
            _database.OtpArticles.Remove(article);
            PopulateList();

            // Keep the list highlight near where it was
            if (TokenListBox.Items.Count > 0)
                TokenListBox.SelectedIndex = Math.Min(removedIdx, TokenListBox.Items.Count - 1);
        }

        // -- Close -----------------------------------------------------------------

        private void CloseButton_Click(object sender, EventArgs e)
        {
            // DialogResult is left as None so the caller can distinguish
            // "user closed manager" from nothing — but we don't need Cancel.
            this.Close();
        }

        // -- Misc -----------------------------------------------------------------

        private void TokenListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        private void TokenListBox_DoubleClick(object sender, EventArgs e)
        {
            SelectButton_Click(sender, e);
        }

        private void TokenManagerWindow_Load(object sender, EventArgs e)
        {

        }

        private void AddUriButton_Click(object sender, EventArgs e)
        {
            string uri = UriAddTextBox.Text;
            if (string.IsNullOrWhiteSpace(uri))
            {
                MessageBox.Show("A valid URI must be entered.", "URI Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TryAddArticleFromUri(uri);
        }

        private void OptTOTP_CheckedChanged(object sender, EventArgs e)
        {
            if (OptTOTP.Checked) { UpdateCounterOrPeriodLabelAndTextBox(); }
        }

        private void OptHOTP_CheckedChanged(object sender, EventArgs e)
        {
            if (OptHOTP.Checked) { UpdateCounterOrPeriodLabelAndTextBox(); }
        }

        private void UpdateCounterOrPeriodLabelAndTextBox()
        {
            if (OptTOTP.Checked)
            {
                CounterOrPeriodLabel.Text = "Period (s):";
                CountOrPeriodTextBox.Text = "30";
            }
            else if (OptHOTP.Checked)
            {
                CounterOrPeriodLabel.Text = "Counter Value:";
                CountOrPeriodTextBox.Text = "0";
            }
        }
    }
}