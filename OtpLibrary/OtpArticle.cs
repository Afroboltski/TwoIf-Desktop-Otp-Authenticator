using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Web;
using OtpLibrary;
using OtpNet;
using static OtpLibrary.PasswordEncryptor;

namespace OtpLibrary
{
    public class OtpArticle
    {
        private static readonly bool ALWAYS_ASK_TO_EDIT_NAME_AND_ACCOUNT_INFO = false;
        private readonly string _uri;

        private readonly Totp _totp;   // non-null when type == totp
        private readonly Hotp _hotp;   // non-null when type == hotp

        // Properties
        public string Name { get; }
        public string Account { get; }
        public OtpType Type { get; }  // Totp or Hotp
        public OtpHashMode Algorithm { get; }
        public int Digits { get; }
        public int Period { get; }  // seconds; TOTP only (0 for HOTP)
        public long InitialHOTPCounter { get; private set; }
        public long HOTPCounter { get; set; }

        /// <summary>Returns the current OTP code, or null if not initialised.</summary>
        public string CalculateCode()
        {
            if (_totp != null) return _totp.ComputeTotp();
            if (_hotp != null) return _hotp.ComputeHOTP(HOTPCounter);
            return null;
        }



        /// <summary>
        /// Seconds remaining in the current TOTP window.
        /// Returns -1 for HOTP (counter-based, no time window).
        /// </summary>
        public int GetRemainingTime()
        {
            if (_totp != null) return _totp.RemainingSeconds();
            return -1;
        }

        // -- Constructor -----------------------------------------------------------

        public OtpArticle(string uri, Func<string,string,Tuple<string,string>> callbackToModifyName = null)
        {
            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentException("URI must not be empty.", nameof(uri));

            _uri = uri;

            ParsedOtpUri parsed = ParseUri(uri);

            // Determine an account name to use
            // If we have an account name, I guess that is important?
            if(!string.IsNullOrWhiteSpace(parsed.Issuer))
            {
                Name = parsed.Issuer;
            }
            else if(!string.IsNullOrWhiteSpace(parsed.IssuerLabel))
            {
                Name = parsed.IssuerLabel;
            }
            else if(!string.IsNullOrWhiteSpace(parsed.Account))
            {
                Name = parsed.Account;
            }
            else
            {
                Name = null;
            }
            
            if(!string.IsNullOrWhiteSpace(parsed.Account))
            {
                Account = parsed.Account;
            }
            else
            {
                Account = null;
            }
            
            if(Name==null || ALWAYS_ASK_TO_EDIT_NAME_AND_ACCOUNT_INFO)
            {
                if(callbackToModifyName!=null)
                {
                    Tuple<string,string> userEnteredNameAccount = callbackToModifyName(Name,Account);
                    Name = userEnteredNameAccount.Item1;
                    Account = userEnteredNameAccount.Item2;
                }
                else
                {
                    Name = string.Empty;
                    Account = string.Empty;
                }
            }
            
            if(Name==null) { Name = string.Empty; }
            if(Account==null) { Account = string.Empty; }
            
            Type      = parsed.Type;
            Algorithm = parsed.Algorithm;
            Digits    = parsed.Digits;
            Period    = parsed.Period;

            byte[] secretBytes = Base32Encoding.ToBytes(parsed.Secret);

            if (parsed.Type == OtpType.Totp) //Totp
            {
                _totp = new Totp(
                    secretBytes,
                    step:      parsed.Period,
                    mode:      parsed.Algorithm,
                    totpSize:  parsed.Digits);
            }
            else // Hotp
            {
                InitialHOTPCounter = parsed.Counter;
                HOTPCounter = InitialHOTPCounter;
                _hotp = new Hotp(
                    secretBytes,
                    mode:     parsed.Algorithm,
                    hotpSize: parsed.Digits);
            }
        }

        public static OtpArticle FromBlob(string blob, string password)
        {
            try
            {
                string uri = PasswordEncryptor.DecryptFromBase64(blob, password);
                return new OtpArticle(uri);
            }
            catch (IncorrectEncryptionPasswordException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    "Decryption of blob failed. InnerException: "
                    + ex.GetType().Name + " - " + ex.Message, ex);
            }
        }

#if DEBUG
        private static readonly bool WRITE_UNENCRYPTED_URI_TO_FILE = false;
#endif
        public string ToBlob(string password)
        {
            if (string.IsNullOrEmpty(password)) return null;
            try
            {
                string result = PasswordEncryptor.EncryptToBase64(_uri, password);
#if DEBUG
                if (WRITE_UNENCRYPTED_URI_TO_FILE) { result = result + Environment.NewLine + _uri; }
#endif
                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    "Encryption of blob failed. InnerException: "
                    + ex.GetType().Name + " - " + ex.Message, ex);
            }
        }

        public string ToBase64()
        {
            try
            {
                string result = Convert.ToBase64String(Encoding.UTF8.GetBytes(_uri));
                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    "Encoding of Base64 string failed. InnerException: "
                    + ex.GetType().Name + " - " + ex.Message, ex);
            }
        }

        public static OtpArticle FromBase64(string base64)
        {
            try
            {
                string uri = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                return new OtpArticle(uri);
            }
            catch (IncorrectEncryptionPasswordException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    "Decoding of Base64 string failed. InnerException: "
                    + ex.GetType().Name + " - " + ex.Message, ex);
            }
        }

        private sealed class ParsedOtpUri
        {
            public OtpType     Type        { get; set; } = OtpType.Totp;
            public string      Secret      { get; set; }
            public string      Issuer      { get; set; }      // from query param
            public string      IssuerLabel { get; set; }      // from path prefix
            public string      Account     { get; set; }      // from path suffix
            public OtpHashMode Algorithm   { get; set; } = OtpHashMode.Sha1;
            public int         Digits      { get; set; } = 6;
            public int         Period      { get; set; } = 30;
            public long        Counter     { get; set; } = 0;
        }

        private static ParsedOtpUri ParseUri(string input)
        {
            var result = new ParsedOtpUri();

            // Legacy: bare secret string (no scheme)
            if (!input.Contains("://"))
            {
                result.Secret = input.Trim();
                return result;
            }

            // Full otpauth URI
            Uri uri;
            try { uri = new Uri(input); }
            catch (Exception ex)
            {
                throw new FormatException("The OTP URI is malformed: " + ex.Message, ex);
            }

            if (!string.Equals(uri.Scheme, "otpauth", StringComparison.OrdinalIgnoreCase))
                throw new FormatException($"Unrecognised URI scheme '{uri.Scheme}'. Expected 'otpauth'.");

            // Type (host segment)
            string host = uri.Host.ToLowerInvariant();
            if (host == "totp")       result.Type = OtpType.Totp;
            else if (host == "hotp")  result.Type = OtpType.Hotp;
            else throw new FormatException($"Unrecognised OTP type '{uri.Host}'. Expected 'totp' or 'hotp'.");

            // Label (path): [issuer:]account
            // The path starts with a '/', so AbsolutePath is e.g. "/ACME:alice"
            string path = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/'));
            int colonIdx = path.IndexOf(':');
            if (colonIdx >= 0)
            {
                result.IssuerLabel = path.Substring(0, colonIdx).Trim();
                result.Account     = path.Substring(colonIdx + 1).Trim();
            }
            else
            {
                result.Account = path.Trim();
            }

            // Query parameters
            var query = HttpUtility.ParseQueryString(uri.Query);

            // Secret (required)
            string secret = query["secret"];
            if (string.IsNullOrWhiteSpace(secret))
                throw new FormatException("The OTP URI does not contain a 'secret' parameter.");
            result.Secret = secret.Trim().ToUpperInvariant(); // Base32 is case-insensitive

            // Algorithm (optional, default SHA1)
            string algorithm = query["algorithm"];
            if (!string.IsNullOrWhiteSpace(algorithm))
            {
                result.Algorithm = algorithm.ToUpperInvariant() switch
                {
                    "SHA1"   => OtpHashMode.Sha1,
                    "SHA256" => OtpHashMode.Sha256,
                    "SHA512" => OtpHashMode.Sha512,
                    _ => throw new FormatException(
                            $"Unrecognised algorithm '{algorithm}'. Expected SHA1, SHA256, or SHA512.")
                };
            }

            // Digits (optional, default 6, valid 6-8)
            string digits = query["digits"];
            if (!string.IsNullOrWhiteSpace(digits))
            {
                if (!int.TryParse(digits, out int d) || d < 6 || d > 8)
                    throw new FormatException(
                        $"Invalid 'digits' value '{digits}'. Must be 6, 7, or 8.");
                result.Digits = d;
            }

            // period (optional, TOTP only, default 30, must be positive)
            string period = query["period"];
            if (!string.IsNullOrWhiteSpace(period))
            {
                if (!int.TryParse(period, out int p) || p <= 0)
                    throw new FormatException(
                        $"Invalid 'period' value '{period}'. Must be a positive integer.");
                result.Period = p;
            }

            // counter (required for HOTP, ignored for TOTP)
            string counter = query["counter"];
            if (!string.IsNullOrWhiteSpace(counter))
            {
                if (!long.TryParse(counter, out long c) || c < 0)
                    throw new FormatException(
                        $"Invalid 'counter' value '{counter}'. Must be a non-negative integer.");
                result.Counter = c;
            }
            else if (result.Type == OtpType.Hotp)
            {
                // RFC 4226 section 5 says the counter MUST be present for HOTP.
                // We allow it to be absent and default to 0 rather than hard-failing,
                // since some poorly-formed URIs omit it.
                result.Counter = 0;
            }

            // issuer (optional display name, takes priority over label prefix)
            string issuer = query["issuer"];
            if (!string.IsNullOrWhiteSpace(issuer))
                result.Issuer = Uri.UnescapeDataString(issuer).Trim();

            return result;
        }
    }
}