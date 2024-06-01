using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace BlogApi.Utils
{
    public class StringUtil
    {
        /// <summary>
        /// Converts a string to Title Case
        /// </summary>
        /// <param name="str">The string to convert</param>
        /// <returns>A string that is already in Title Case format</returns>
        public static string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// Hash a plain text to make it secure
        /// </summary>
        /// <param name="str">The string to hash</param>
        /// <param name="saltVal"></param>
        /// <returns>A string that is hashed already</returns>
        public static string HashPlainText(string str, byte[]? saltVal = null)
        {
            // Generate a 128-bit salt using a sequence of
            // cryptographically strong random bytes.
            byte[] salt = saltVal ?? RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: str!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        /// <summary>
        /// Gets the current datetime
        /// </summary>
        /// <returns>The current datetime</returns>
        public static string GetCurrentDateTime()
        {
            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
            string currentDateTime = time.ToString(format);

            return currentDateTime;
        }
    }
}
