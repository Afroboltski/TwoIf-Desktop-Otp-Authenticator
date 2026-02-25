using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Security.Principal;
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

        public TokenManagerWindow(OtpDatabase database, int backcolorIntensity = 24)
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(backcolorIntensity, backcolorIntensity, backcolorIntensity);

            this.CloseButton.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.AddQrButton.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.AddUriButton.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.AddManuallyButton.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.DeleteButton.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.SelectButton.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.ChangePasswordButton.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.NameTextBox.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.AccountTextBox.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.SecretTextBox.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.CountOrPeriodTextBox.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.UriAddTextBox.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);
            this.TokenListBox.BackColor = Color.FromArgb(backcolorIntensity + 16, backcolorIntensity + 16, backcolorIntensity + 16);


            _database = database;
            PopulateList();
        }

        private void PopulateList()
        {
            int previousListSelection = TokenListBox.SelectedIndex;

            TokenListBox.Items.Clear();
            for (int i = 0; i < _database.OtpArticles.Count; i++)
            {
                var article = _database.OtpArticles[i];
                bool isActive = (article == _database.SelectedArticle);
                // Green check for the active token, blank indent for others
                TokenListBox.Items.Add((isActive ? "✔\t" : "\t") + article.Name);
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

        private void ChangePasswordButton_Click(object sender, EventArgs e)
        {
            string newPassword;
            using (var newDlg = new PasswordDialog("Enter your NEW database password:"))
            {
                if (newDlg.ShowDialog(this) != DialogResult.OK) return;
                newPassword = newDlg.Password;
            }

            using (var confirmDlg = new PasswordDialog("Confirm your NEW database password:"))
            {
                if (confirmDlg.ShowDialog(this) != DialogResult.OK) return;
                if (confirmDlg.Password != newPassword)
                {
                    MessageBox.Show(
                        "The new passwords do not match. Password was not changed.",
                        "Password Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                _database.Password = newPassword;
                AppDataStore.Save(OtpDatabase.DEFAULT_DATABASE_FILE_NAME, _database, _database.Password);
                MessageBox.Show("Password changed successfully.",
                    "Password Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred:\n\n"
                    + ex.GetType().Name + " — " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private bool TryAddArticleFromUri(string uri)
        {
            OtpArticle article;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                article = new OtpArticle(uri);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not parse token data." + Environment.NewLine + Environment.NewLine + "" + ex.Message,
                    "Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            if (string.IsNullOrWhiteSpace(article.Name))
            {
                // Malformed/legacy URI? Prompt for a Name and (optional) account.

                string userSuppliedName = null;
                string userSuppliedAccount = null;

                using var nameDlg = new TokenNameDialog(this.BackColor.R);
                if (nameDlg.ShowDialog(this) != DialogResult.OK)
                    return false; // user cancelled — don't add a nameless token

                userSuppliedName = nameDlg.TokenName;
                userSuppliedAccount = nameDlg.TokenAccount;

                bool applied = article.SetNameIfBlank(userSuppliedName, userSuppliedAccount);
                if(!applied)
                {
                    MessageBox.Show("Failed to apply name to the imported token. Import will not continue.",
                    "Token Name Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                _database.OtpArticles.Add(article);
                _database.SelectedArticle = article;
                AppDataStore.Save(OtpDatabase.DEFAULT_DATABASE_FILE_NAME, _database, _database.Password);

                // Highlight the newly added row
                int newIdx = _database.OtpArticles.Count - 1;
                PopulateList();
                TokenListBox.SelectedIndex = newIdx;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            return true;
        }

        // -- Use Selected ----------------------------------------------------------

        private void SelectButton_Click(object sender, EventArgs e)
        {
            var article = CurrentlyHighlightedArticle;
            if (article == null) return;

            _database.SelectedArticle = article;
            AppDataStore.SaveHeaderOnly(OtpDatabase.DEFAULT_DATABASE_FILE_NAME, _database);

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

            if (article == _database.SelectedArticle)
                _database.SelectedArticle = null; // active token is gone

            int removedIdx = TokenListBox.SelectedIndex;
            

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                _database.OtpArticles.Remove(article);
                AppDataStore.Save(OtpDatabase.DEFAULT_DATABASE_FILE_NAME, _database, _database.Password);
                PopulateList();

                // Keep the list highlight near where it was
                if (TokenListBox.Items.Count > 0)
                    TokenListBox.SelectedIndex = Math.Min(removedIdx, TokenListBox.Items.Count - 1);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
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