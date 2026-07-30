using System;
using System.Security.Cryptography;
using System.Text;

namespace Services
{
    public static class PasswordHelper
    {
        // Genera una sal aleatoria
        public static string GenerarSalt(int size = 16)
        {
            var rng = new RNGCryptoServiceProvider();
            var saltBytes = new byte[size];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        
        public static string GenerarPasswordHash(string password, string salt)
        {
            var combined = password + salt;
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(combined);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}