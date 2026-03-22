using System;
using System.Security.Cryptography;

namespace RMS_UI.Utilities
{
    public static class PasswordSecurity
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100_000;

        public static (string Hash, string Salt) CreateHashAndSalt(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = HashPassword(password, salt);

            return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }

        public static bool Verify(string password, string hashBase64, string saltBase64)
        {
            if (string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(hashBase64) ||
                string.IsNullOrWhiteSpace(saltBase64))
            {
                return false;
            }

            try
            {
                byte[] salt = Convert.FromBase64String(saltBase64);
                byte[] expectedHash = Convert.FromBase64String(hashBase64);
                byte[] actualHash = HashPassword(password, salt);

                return CryptographicOperations.FixedTimeEquals(expectedHash, actualHash);
            }
            catch
            {
                return false;
            }
        }

        private static byte[] HashPassword(string password, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256);

            return pbkdf2.GetBytes(HashSize);
        }
    }
}
