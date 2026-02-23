using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using OtpLibrary;
using OtpNet;
using static OtpLibrary.PasswordEncryptor;

namespace OtpLibrary
{
    public class OtpArticle
    {
        private string _uriCache = null;

        private readonly Totp _totp;   // non-null when type == totp
        private readonly Hotp _hotp;   // non-null when type == hotp

        // Properties
        public string Name { get; private set; }
        public string Account { get; private set; }
        public OtpType Type { get; }  // Totp or Hotp
        public OtpHashMode Algorithm { get; }
        public int Digits { get; }
        public int Period { get; }  // seconds; TOTP only (0 for HOTP)
        public long InitialHOTPCounter { get; private set; }
        public long HOTPCounter { get; set; }
        public string Secret { get; }
        public string IssuerLabel { get; private set; }
        public string Issuer { get; }

        private string TokenUri
        {
            get
            {
                if (_uriCache != null) return _uriCache;
                long periodOrCounter = (long)Period;
                if (Type == OtpType.Hotp) { periodOrCounter = HOTPCounter; }
                _uriCache = CreateUri(IssuerLabel,Secret,Type,Account,Issuer,Algorithm.ToString(),Digits,periodOrCounter);
                return _uriCache;
            }
        }

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

        public OtpArticle(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentException("URI must not be empty.", nameof(uri));

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
            
            if(Name==null) { Name = string.Empty; }
            if(Account==null) { Account = string.Empty; }
            
            Type        = parsed.Type;
            Algorithm   = parsed.Algorithm;
            Digits      = parsed.Digits;
            Period      = parsed.Period;
            Secret      = parsed.Secret;
            Issuer      = parsed.Issuer;
            IssuerLabel = parsed.IssuerLabel;

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

        public bool SetNameIfBlank(string name, string account = null)
        {
            if(!string.IsNullOrWhiteSpace(this.Name))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            this.Name = name;
            this.IssuerLabel = name;
            if (!string.IsNullOrWhiteSpace(account))
            {
                this.Account = account;
            }

            return true;
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

        public string ToBlob(string password)
        {
            if (string.IsNullOrEmpty(password)) return null;
            try
            {
                string result = PasswordEncryptor.EncryptToBase64(TokenUri, password);
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
                string result = Convert.ToBase64String(Encoding.UTF8.GetBytes(TokenUri));
                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    "Encoding of Base64 string failed. InnerException: "
                    + ex.GetType().Name + " - " + ex.Message, ex);
            }
        }

        public string ToUri()
        {
            return TokenUri;
        }

        public static OtpArticle FromBase64(string base64)
        {
            try
            {
                string uri = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                return new OtpArticle(uri);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    "Decoding of Base64 string failed. InnerException: "
                    + ex.GetType().Name + " - " + ex.Message, ex);
            }
        }

        public static OtpArticle FromUri(string uri)
        {
            return new OtpArticle(uri);
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

        private static readonly int MINIMUM_DIGITS = 6;
        private static readonly int MAXIMUM_DIGITS = 8;

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
                result.IssuerLabel = path.Trim();
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
            {
                result.Issuer = Uri.UnescapeDataString(issuer).Trim();
                if(!result.Issuer.Equals(result.IssuerLabel, StringComparison.OrdinalIgnoreCase) && string.IsNullOrWhiteSpace(result.Account))
                {
                    result.Account = result.IssuerLabel;
                }
            }
            return result;
        }

        public static string CreateUri(string name, string secret, OtpType otpType = OtpType.Totp, string account = null, string issuer = null, string hashModeStringInput = null, int digits = -1, long periodOrCounter = -1)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("otpauth://");
            string otpTypeString = otpType switch
            {
                OtpType.Totp => "totp",
                OtpType.Hotp => "hotp",
                _ => null
            };
            if (otpTypeString == null) throw new FormatException("Invalid URI parameter: OTP type");
            sb.Append(otpTypeString); sb.Append('/');
            sb.Append(name);
            if (!string.IsNullOrWhiteSpace(account)) { sb.Append(':'); sb.Append(account); }
            sb.Append("?secret=");
            sb.Append(secret);
            if (hashModeStringInput != null)
            {
                OtpHashMode hashMode = OtpHashMode.Sha1;
                if (Regex.IsMatch(hashModeStringInput.Trim(), @"(SHA)?1$", RegexOptions.IgnoreCase)) { hashMode = OtpHashMode.Sha1; }
                else if (Regex.IsMatch(hashModeStringInput.Trim(), @"(SHA)?256$", RegexOptions.IgnoreCase)) { hashMode = OtpHashMode.Sha256; }
                else if (Regex.IsMatch(hashModeStringInput.Trim(), @"(SHA)?512$", RegexOptions.IgnoreCase)) { hashMode = OtpHashMode.Sha512; }
                string hashModeString = hashMode switch
                {
                    OtpHashMode.Sha1 => "SHA1",
                    OtpHashMode.Sha256 => "SHA256",
                    OtpHashMode.Sha512 => "SHA512",
                    _ => null
                };
                if (hashModeString == null) throw new FormatException("Invalid URI parameter: hash algorithm type");
                sb.Append("&algorithm="); sb.Append(hashModeString);
            }
            if (digits >= 0) { if (digits > MAXIMUM_DIGITS || digits < MINIMUM_DIGITS) { throw new FormatException("Invalid URI parameter: digits"); } sb.Append("&digits="); sb.Append(digits.ToString()); }
            string periodOrCounterParam = "&period=";
            if (otpType == OtpType.Hotp) { periodOrCounterParam = "&counter="; }
            if (periodOrCounter >= 0) { sb.Append(periodOrCounterParam); sb.Append(periodOrCounter.ToString()); }
            if (!string.IsNullOrWhiteSpace(issuer)) { sb.Append("&issuer="); sb.Append(issuer.Trim()); }
            return sb.ToString();
        }

        
    }
}