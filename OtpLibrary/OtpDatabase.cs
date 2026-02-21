using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtpLibrary
{
    public class OtpDatabase
    {
        public List<OtpArticle> OtpArticles { get; set; }

        /// <summary>
        /// Index of the article the user last selected as active.
        /// -1 means none selected.
        /// </summary>
        public int SelectedIndex { get; set; } = -1;

        /// <summary>Convenience accessor — null if SelectedIndex is out of range.</summary>
        public OtpArticle SelectedArticle
        {
            get
            {
                if (SelectedIndex < 0 || SelectedIndex >= OtpArticles.Count)
                    return null;
                return OtpArticles[SelectedIndex];
            }
            set
            {
                SelectedIndex = value == null ? -1 : OtpArticles.IndexOf(value);
            }
        }

        public OtpDatabase() { OtpArticles = new List<OtpArticle>(); }

        // ── Serialisation ────────────────────────────────────────────────────────
        // File format:
        //   Line 0:  "SEL:<decimal index>"   (new header line)
        //   Line 1+: one encrypted blob per article  (unchanged)
        //
        // If line 0 is missing or not a SEL header we treat the whole file as
        // legacy (no selection info) so old databases load cleanly.

        private const string SelectionPrefix = "SEL:";
        private static readonly char[] separator = new[] { '\r', '\n' };

        public static OtpDatabase LoadFromFile(string fileName, string password)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("A valid file must be specified.", nameof(fileName));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("A valid password must be specified.", nameof(password));
            if (!File.Exists(fileName))
                throw new ArgumentException("A valid file must be specified.", nameof(fileName));

            string cipherText = File.ReadAllText(fileName);
            string plainText = PasswordEncryptor.DecryptFromBase64(cipherText,password);
            string[] lines = plainText.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            int firstBlobLine = 0;
            int savedSelectedIndex = -1;

            // Check for new-format header
            if (lines.Length > 0 && lines[0].StartsWith(SelectionPrefix))
            {
                if (int.TryParse(lines[0].Substring(SelectionPrefix.Length), out int idx))
                    savedSelectedIndex = idx;
                firstBlobLine = 1;
            }

            int invalidPasswordCount = 0;
            OtpDatabase db = new OtpDatabase();

            for (int i = firstBlobLine; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                OtpArticle article = OtpArticle.FromBase64(lines[i]);
                if (article == null)
                {
                    invalidPasswordCount++;
                    continue;
                }
                db.OtpArticles.Add(article);
            }

            if (invalidPasswordCount > 0 && db.OtpArticles.Count == 0)
                return null; // all blobs failed → wrong password

            // Restore selection, clamping to valid range
            if (savedSelectedIndex >= 0 && savedSelectedIndex < db.OtpArticles.Count)
                db.SelectedIndex = savedSelectedIndex;
            else
                db.SelectedIndex = db.OtpArticles.Count > 0 ? 0 : -1;

            return db;
        }

        public void WriteToFile(string fileName, string password)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("A valid file must be specified.", nameof(fileName));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("A valid password must be specified.", nameof(password));

            var sb = new StringBuilder();

            // Header line — always written so future loads know the selection
            sb.AppendLine(SelectionPrefix + SelectedIndex);

            for (int i = 0; i < OtpArticles.Count; i++)
            {
                if (OtpArticles[i] == null) continue;
                sb.AppendLine(OtpArticles[i].ToBase64());
            }

            string ciphertext = PasswordEncryptor.EncryptToBase64(sb.ToString(), password);

            File.WriteAllText(fileName, ciphertext);
        }
    }
}