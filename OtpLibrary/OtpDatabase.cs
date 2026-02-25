using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static OtpLibrary.PasswordEncryptor;

namespace OtpLibrary
{
    public class OtpDatabase
    {
        public static readonly string DEFAULT_DATABASE_FILE_NAME = "database.dat";
        private SecureString _password = null;

        private readonly byte[] _temporaryExtraScrambling = new byte[32];

        public string Password
        {
            get
            {
                if (_password == null)
                    return null;
                IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(_password);
                try
                {
                    string base64Password = System.Runtime.InteropServices.Marshal.PtrToStringUni(ptr);
                    byte[] stringBytes = Convert.FromBase64String(base64Password);
                    for (int j = 0; j < stringBytes.Length; j++)
                    {
                        for (int i = 0; i < _temporaryExtraScrambling.Length; i++)
                        {
                            stringBytes[j] = (byte)(stringBytes[j] ^ _temporaryExtraScrambling[i]);
                        }
                    }
                    return Encoding.UTF8.GetString(stringBytes);
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(ptr);
                }
            }
            set
            {
                _password = new System.Security.SecureString();
                byte[] stringBytes = Encoding.UTF8.GetBytes(value);
                for(int j=0;j< stringBytes.Length;j++)
                {
                    for (int i = 0; i < _temporaryExtraScrambling.Length; i++)
                    {
                        stringBytes[j] = (byte)(stringBytes[j] ^ _temporaryExtraScrambling[i]);
                    }
                }
                string base64Password = Convert.ToBase64String(stringBytes);

                foreach (char c in base64Password)
                {
                    _password.AppendChar(c);
                }
                _password.MakeReadOnly();
            }
        }

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

        public OtpDatabase()
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(_temporaryExtraScrambling);
            }
            OtpArticles = new List<OtpArticle>();
        }

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

            ReadFile(fileName, out string header, out string cipherText);
            string plainText = null;

            try
            {
                plainText = PasswordEncryptor.DecryptFromBase64(cipherText, password);
            }
            catch (IncorrectEncryptionPasswordException)
            {
                return null;
            }

            string[] lines = plainText.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            int savedSelectedIndex = -1;
            int skipLines = 0;

            // Check for new-format header
            if (!string.IsNullOrEmpty(header) && header.StartsWith(SelectionPrefix))
            {
                if (int.TryParse(header.Substring(SelectionPrefix.Length), out int idx))
                    savedSelectedIndex = idx;
            }
            else if (lines!=null && lines.Length > 0 && !string.IsNullOrEmpty(lines[0]) && lines[0].StartsWith(SelectionPrefix))
            {
                if (int.TryParse(lines[0].Substring(SelectionPrefix.Length), out int idx))
                {
                    savedSelectedIndex = idx;
                    skipLines = 1;
                }

            }

            int invalidPasswordCount = 0;
            OtpDatabase db = new OtpDatabase();

            for (int i = skipLines; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                if (lines[i].StartsWith(SelectionPrefix)) 
                {
                    // Perhaps the old format, with a new header?
                    continue;
                }

                OtpArticle article = OtpArticle.FromUri(lines[i]);
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
            string header = SelectionPrefix + SelectedIndex;

            for (int i = 0; i < OtpArticles.Count; i++)
            {
                if (OtpArticles[i] == null) continue;
                sb.AppendLine(OtpArticles[i].ToUri());
            }

            string cipherText = PasswordEncryptor.EncryptToBase64(sb.ToString(), password);

            WriteFile(fileName, header, cipherText);
        }

        public void WriteOnlyHeaderToFile(string fileNameForWriting, string existingFile)
        {
            if (string.IsNullOrWhiteSpace(fileNameForWriting))
                throw new ArgumentException("A valid file must be specified.", nameof(fileNameForWriting));

            ReadFile(existingFile, out string _, out string cipherText);

            // Header line — always written so future loads know the selection
            string header = SelectionPrefix + SelectedIndex;
            WriteFile(fileNameForWriting, header, cipherText);


        }

        private static void WriteFile(string fileName, string header, string cipherText)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("A valid file must be specified.", nameof(fileName));

            using (FileStream fs = File.Create(fileName))
            {
                if(!string.IsNullOrEmpty(header))
                {
                    fs.Write(Encoding.UTF8.GetBytes(header));
                }
                fs.WriteByte((byte)'\0');
                fs.Write(Encoding.UTF8.GetBytes(cipherText));
            }
        }

        private static void ReadFile(string fileName, out string header, out string cipherText)
        {
            header = null;
            cipherText = null;
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("A valid file must be specified.", nameof(fileName));

            byte[] allText = File.ReadAllBytes(fileName);
            int indexOfNull = -1;
            for(int i=0;i<allText.Length;i++)
            {
                if (allText[i]=='\0')
                {
                    indexOfNull = i;
                    break;
                }    
            }
            if(indexOfNull >= 1) // 1 (not 0) because a null for byte 0 would still be a non-existant header
            {
                header = Encoding.UTF8.GetString(allText,0,indexOfNull);
            }
            cipherText = Encoding.UTF8.GetString(allText, indexOfNull + 1, allText.Length-(indexOfNull+1));

        }
    }
}