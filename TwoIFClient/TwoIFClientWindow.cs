using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OtpLibrary;
using OtpNet;
using QRCodeLibrary;

namespace TwoIFClient
{
    public partial class TwoIFClientWindow : Form
    {
        private int _timeRemaining = -1;
        private string _oneTimeCode = string.Empty;
        private OtpDatabase _database = null;

        OtpArticle _otpArticle = null;

        public TwoIFClientWindow()
        {
            _intensityDiff = _maxIntensity - _minIntensity;
            InitializeComponent();
            ChangeArticle(null);

            _tempStoreColor = Hamburger.ForeColor;
            Color c = Color.FromArgb(_minIntensity, _minIntensity, _minIntensity);

            SetTransparencies();
            SetBackgroundColor(c);
        }

        // This method is intended to be called once the external library is integrated
        public void SetOneTimeCode(string code)
        {
            _oneTimeCode = code;
            CodeLabel.Text = _oneTimeCode;
        }

        public void SetTimeRemaining(int timeRemaining)
        {
            _timeRemaining = timeRemaining;
            if (_timeRemaining < 0)
            {
                TimeLabel.Text = string.Empty;
                return;
            }
            TimeLabel.Text = _timeRemaining.ToString();
        }

        private void CodeLabel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_oneTimeCode))
            {
                Clipboard.SetText(_oneTimeCode);
                InitiateFlash(400);
            }
        }

        private void InitiateFlash(int milliseconds = 500)
        {
            FlashTimer.Interval = 10;
            _flashTime = milliseconds;
            _flashStartTickCount = Environment.TickCount64;
            FlashTimer.Start();
        }

        private void ChangeArticle(OtpArticle article)
        {
            _otpArticle = article;

            if (_otpArticle != null)
            {
                Font f = CodeLabel.Font;
                Font fMod = _otpArticle.Digits switch
                {
                    <= 6 => new Font(f.Name, 48F, f.Style, f.Unit, 0),
                    7 => new Font(f.Name, 42F, f.Style, f.Unit, 0),
                    8 => new Font(f.Name, 36F, f.Style, f.Unit, 0),
                    9 => new Font(f.Name, 32F, f.Style, f.Unit, 0),
                    > 9 => new Font(f.Name, 32F - (2F * (_otpArticle.Digits - 9)), f.Style, f.Unit, 0)
                };
                CodeLabel.Font = fMod;

                if (_otpArticle.Type == OtpType.Totp)
                {
                    CountLabel.Visible = false;
                    CountEntry.Visible = false;
                    TimeLabel.Visible = true;
                }
                else if (_otpArticle.Type == OtpType.Hotp)
                {
                    CountLabel.Visible = true;
                    CountEntry.Visible = true;
                    CountEntry.Value = (decimal)_otpArticle.HOTPCounter;
                    CountEntry.Focus();
                    CountEntry.Select(0, CountEntry.Value.ToString().Length);
                    TimeLabel.Visible = false;
                }
                else
                {
                    CountLabel.Visible = false;
                    CountEntry.Visible = false;
                    TimeLabel.Visible = false;
                }
            }
            else
            {

                CountLabel.Visible = false;
                CountEntry.Visible = false;
                TimeLabel.Visible = true;
            }
            RefreshName();
            RefreshCode();
        }

        private void RefreshName()
        {
            if (_otpArticle == null)
            {
                NameLabel.Text = string.Empty;
                AccountLabel.Text = string.Empty;
                return;
            }
            string name = _otpArticle.Name;
            NameLabel.Text = name;
            string account = _otpArticle.Account;
            AccountLabel.Text = account;
        }


        private void SetTransparencies()
        {
            TimeLabel.BackColor = Color.Transparent;
            Hamburger.BackColor = Color.Transparent;
            NameLabel.BackColor = Color.Transparent;
            CodeLabel.BackColor = Color.Transparent;
            CountLabel.BackColor = Color.Transparent;
            AccountLabel.BackColor = Color.Transparent;
        }
        private void SetBackgroundColor(Color c)
        {
            this.BackColor = c;
            CountEntry.BackColor = c;
        }

        private static readonly int _maxIntensity = 160;
        private static readonly int _minIntensity = 16;
        private readonly int _intensityDiff; // Calculated in ctor

        private long _flashStartTickCount = -1;
        private int _flashTime = 0;
        private void FlashTimer_Tick(object sender, EventArgs e)
        {
            int currentTime = (int)(Environment.TickCount64 - _flashStartTickCount);
            if (currentTime <= _flashTime)
            {
                int intensity = _maxIntensity - (_intensityDiff * currentTime / _flashTime);
                Color c = Color.FromArgb(intensity, intensity, intensity);
                SetBackgroundColor(c);

            }
            else
            {
                FlashTimer.Stop();
                Color c = Color.FromArgb(_minIntensity, _minIntensity, _minIntensity);
                SetBackgroundColor(c);

            }
        }

        private void TimeLabel_Click(object sender, EventArgs e)
        {
            CodeLabel_Click(sender, e);
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshCode();
        }

        private void RefreshCode()
        {
            if (_otpArticle == null)
            {
                SetOneTimeCode(string.Empty);
                SetTimeRemaining(-1);
                return;
            }
            int timeRemaining = _otpArticle.GetRemainingTime();
            SetTimeRemaining(timeRemaining);

            string code = _otpArticle.CalculateCode();
            SetOneTimeCode(code);
        }

        private void Hamburger_Click(object sender, EventArgs e)
        {
            // Ensure we have a password before we can save (new-database case)
            if (_database.Password == null)
            {
                string firstPassword = PromptForPassword("Choose a password to protect the database:");
                if (firstPassword == null) return; // user cancelled

                string secondPassword = PromptForPassword("Re-enter the same password:");
                if (secondPassword == null) return; // user cancelled

                if(!string.Equals(firstPassword, secondPassword))
                {
                    MessageBox.Show("Passwords do not match!",
                            "Password Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _database.Password = firstPassword;
            }

            try
            {
                using var manager = new TokenManagerWindow(_database, _otpArticle, _minIntensity + 10);
                manager.ShowDialog(this);
                Cursor.Current = Cursors.WaitCursor;

                // Persist whatever changes were made (adds, deletes, new selection)
                _database.SelectedArticle = manager.SelectedArticle;

                // Apply — null is fine; ChangeArticle handles it
                ChangeArticle(manager.SelectedArticle);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private Color _tempStoreColor;
        private void Hamburger_MouseEnter(object sender, EventArgs e)
        {
            _tempStoreColor = Hamburger.ForeColor;
            Hamburger.ForeColor = Scale(_tempStoreColor, 2.0f);
        }

        private Color Scale(Color c, float scale)
        {
            int r = _tempStoreColor.R;
            int g = _tempStoreColor.G;
            int b = _tempStoreColor.B;

            float actualScale = Math.Min(255.0f / r, Math.Min(255.0f / g, Math.Min(255.0f / b, scale)));

            r = Math.Min(Math.Max(0, (int)(actualScale * r)), 255);
            g = Math.Min(Math.Max(0, (int)(actualScale * g)), 255);
            b = Math.Min(Math.Max(0, (int)(actualScale * b)), 255);

            return Color.FromArgb(r, g, b);

        }

        private void Hamburger_MouseLeave(object sender, EventArgs e)
        {
            Hamburger.ForeColor = _tempStoreColor;
        }

        private void NameLabel_Click(object sender, EventArgs e)
        {
            CodeLabel_Click(sender, e);
        }

        private string PromptForPassword(string prompt)
        {
            using var dlg = new PasswordDialog(prompt, _minIntensity + 10);
            return dlg.ShowDialog() == DialogResult.OK ? dlg.Password : null;
        }

        private void TwoIFClientWindow_Load(object sender, EventArgs e)
        {
            if (!AppDataStore.IsPresent(OtpDatabase.DEFAULT_DATABASE_FILE_NAME))
            {
                _database = new OtpDatabase();
                return;
            }

            int passwordAttempts = 0;
            while (true)
            {
                string password = PromptForPassword("Enter database password:");
                if (password == null) { this.Close(); return; }

                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    _database = AppDataStore.Load(OtpDatabase.DEFAULT_DATABASE_FILE_NAME, password);
                    if (_database == null)
                    {
                        DialogResult createNewDueToAccessError = MessageBox.Show("Failed to load database for an unknown reason." + Environment.NewLine + Environment.NewLine + "Open with a fresh (empty) database?",
                            "Database Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (createNewDueToAccessError == DialogResult.Yes)
                        {
                            _database = new OtpDatabase();
                            return;
                        }
                    }
                    _database.Password = password;
                    break;
                }
                catch(InvalidPasswordException ex1)
                {
                    if(passwordAttempts >= 3)
                    {
                        _database = null;
                        break;
                    }
                    var retry = MessageBox.Show("Incorrect password." + Environment.NewLine + Environment.NewLine + "Try again?",
                            "Wrong Password", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (retry == DialogResult.Yes)
                    {
                        passwordAttempts++;
                        continue;
                    }
                    _database = null;
                    break;
                }
                catch (Exception ex2)
                {
                    var sb = new System.Text.StringBuilder();
                    sb.AppendLine("Error opening database!");
                    sb.AppendLine();
                    sb.AppendLine(ex2.GetType().Name + " - " + ex2.Message);
                    if (ex2.InnerException != null)
                        sb.AppendLine("Inner: " + ex2.InnerException.GetType().Name
                                      + " - " + ex2.InnerException.Message);
                    sb.AppendLine();
                    sb.AppendLine("Open with a fresh (empty) database?");
                    var res = MessageBox.Show(sb.ToString(), "Error",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (res == DialogResult.Yes) { _database = new OtpDatabase(); break; }
                    this.Close(); return;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }

            if (_database == null) { this.Close(); return; }

            try
            {
                Cursor = Cursors.WaitCursor;
                // Restore whichever article was active when the database was last saved
                ChangeArticle(_database?.SelectedArticle);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void CountLabel_Click(object sender, EventArgs e)
        {
            CodeLabel_Click(sender, e);
        }

        private bool _supress_next_CountEntry_ValueChanged = false;
        private void CountEntry_ValueChanged(object sender, EventArgs e)
        {
            if (_supress_next_CountEntry_ValueChanged)
            {
                _supress_next_CountEntry_ValueChanged = false;
                return;
            }
            try
            {
                _otpArticle.HOTPCounter = (long)CountEntry.Value;
            }
            catch
            {
                _supress_next_CountEntry_ValueChanged = true;
                CountEntry.Value = (decimal)_otpArticle.InitialHOTPCounter;
                _otpArticle.HOTPCounter = _otpArticle.InitialHOTPCounter;
            }
        }

        private void AccountLabel_Click(object sender, EventArgs e)
        {
            CodeLabel_Click(sender, e);
        }

        private void CountEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                CountEntry.Focus();
                CountEntry.Select(0, CountEntry.Value.ToString().Length);
            }
            else
            {
                ApplyCounterTimer.Stop();
                ApplyCounterTimer.Start();
            }

        }

        private void ApplyCounterTimer_Tick(object sender, EventArgs e)
        {
            ApplyCounterTimer.Stop();
            CountEntry.Focus();
        }

        private void CountEntry_Leave(object sender, EventArgs e)
        {
            ApplyCounterTimer.Stop();
        }

    }
}
