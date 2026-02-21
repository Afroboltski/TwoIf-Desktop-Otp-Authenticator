using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static OtpLibrary.PasswordEncryptor;

namespace OtpLibrary
{
    public static class PasswordEncryptor
    {
        private const int ITERATIONS = 600_000;
        private const int SALT_SIZE = 16;

        // Plaintext + password -> Base64
        public static string EncryptToBase64(string plaintext, string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SALT_SIZE);

            // 32 bytes AES + 32 bytes HMAC
            byte[] keyMaterial = DeriveKey(
                password,
                salt,
                keyBytes: 64,
                iterations: ITERATIONS);

            byte[] aesKey = keyMaterial[..32];
            byte[] hmacKey = keyMaterial[32..];

            EncryptedPayload payload = Encrypt(
                Encoding.UTF8.GetBytes(plaintext),
                aesKey,
                hmacKey);

            // Serialize: salt || iv || ciphertext || hmac
            byte[] blob = Concat(
                salt,
                payload.IV,
                payload.Ciphertext,
                payload.Hmac);

            return Convert.ToBase64String(blob);
        }

        // Base64 + password -> plaintext
        public static string DecryptFromBase64(string base64, string password)
        {
            byte[] blob = Convert.FromBase64String(base64);

            byte[] salt = blob[..SALT_SIZE];
            byte[] iv = blob[SALT_SIZE..(SALT_SIZE + 16)];
            byte[] hmac = blob[^32..];
            byte[] ciphertext = blob[
                (SALT_SIZE + 16)..(blob.Length - 32)];

            byte[] keyMaterial = DeriveKey(
                password,
                salt,
                keyBytes: 64,
                iterations: ITERATIONS);

            byte[] aesKey = keyMaterial[..32];
            byte[] hmacKey = keyMaterial[32..];

            var payload = new EncryptedPayload
            {
                IV = iv,
                Ciphertext = ciphertext,
                Hmac = hmac
            };

            byte[] plaintext = Decrypt(
                payload,
                aesKey,
                hmacKey);

            return Encoding.UTF8.GetString(plaintext);
        }

        // Encrypt-then-MAC (AES-256-CBC + HMAC-SHA256)
        private static EncryptedPayload Encrypt(
            byte[] plaintext,
            byte[] aesKey,
            byte[] hmacKey)
        {
            if (aesKey.Length != 32) throw new ArgumentException("AES key must be 32 bytes");
            if (hmacKey.Length < 32) throw new ArgumentException("HMAC key too short");

            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = aesKey;
            aes.GenerateIV();

            byte[] ciphertext;
            using (var enc = aes.CreateEncryptor())
            {
                ciphertext = enc.TransformFinalBlock(plaintext, 0, plaintext.Length);
            }

            // MAC = HMAC(key, IV || ciphertext)
            byte[] mac;
            using (var hmac = new HMACSHA256(hmacKey))
            {
                mac = hmac.ComputeHash(Concat(aes.IV, ciphertext));
            }

            return new EncryptedPayload
            {
                IV = aes.IV,
                Ciphertext = ciphertext,
                Hmac = mac
            };
        }

        private static byte[] Decrypt(
            EncryptedPayload payload,
            byte[] aesKey,
            byte[] hmacKey)
        {
            if (aesKey.Length != 32) throw new ArgumentException("AES key must be 32 bytes");
            if (hmacKey.Length < 32) throw new ArgumentException("HMAC key too short");

            // Verify MAC *before* decryption
            using (var hmac = new HMACSHA256(hmacKey))
            {
                var expected = hmac.ComputeHash(
                    Concat(payload.IV, payload.Ciphertext));

                if (!CryptographicOperations.FixedTimeEquals(expected, payload.Hmac))
                    throw new IncorrectEncryptionPasswordException(new CryptographicException("Authentication failed"));
            }

            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = aesKey;
            aes.IV = payload.IV;

            using var dec = aes.CreateDecryptor();
            return dec.TransformFinalBlock(
                payload.Ciphertext, 0, payload.Ciphertext.Length);
        }

        private static byte[] Concat(params byte[][] parts)
        {
            int len = 0;
            foreach (var p in parts) len += p.Length;

            var result = new byte[len];
            int offset = 0;

            foreach (var p in parts)
            {
                Buffer.BlockCopy(p, 0, result, offset, p.Length);
                offset += p.Length;
            }

            return result;
        }
        // RFC 2898 (PBKDF2-HMAC-SHA256)
        private static byte[] DeriveKey(
            string password,
            out byte[] salt,
            int keyBytes = 32,
            int iterations = 600_000)
        {
            salt = RandomNumberGenerator.GetBytes(16);

            using var kdf = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256);

            return kdf.GetBytes(keyBytes);
        }

        private static byte[] DeriveKey(
            string password,
            byte[] salt,
            int keyBytes = 32,
            int iterations = 600_000)
        {
            using var kdf = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256);

            return kdf.GetBytes(keyBytes);
        }

        public class IncorrectEncryptionPasswordException : CryptographicException
        {
            public CryptographicException CryptographicException { get; private set; }
            public IncorrectEncryptionPasswordException(CryptographicException cryptographicException)
            {
                CryptographicException = cryptographicException;
            }
        }

        private sealed class EncryptedPayload
        {
            public byte[] IV { get; init; }
            public byte[] Ciphertext { get; init; }
            public byte[] Hmac { get; init; }
        }
    }
}



    
