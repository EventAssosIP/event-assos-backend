using System.Security.Cryptography;
using System.Text;

namespace EventAssos.Application.Services.Tools
{
    public static class PasswordGenerator
    {
        private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
        private const string Digits = "0123456789";
        private const string AllChars = Uppercase + Lowercase + Digits;

        public static string Generate(int length = 10)
        {
            if (length < 8) length = 8; // Sécurité minimale

            // On s'assure d'avoir au moins un de chaque pour valider ton Regex
            var password = new StringBuilder();
            password.Append(Uppercase[RandomNumberGenerator.GetInt32(Uppercase.Length)]);
            password.Append(Digits[RandomNumberGenerator.GetInt32(Digits.Length)]);

            // On remplit le reste aléatoirement
            for (int i = 2; i < length; i++)
            {
                password.Append(AllChars[RandomNumberGenerator.GetInt32(AllChars.Length)]);
            }

            // On mélange pour que la majuscule et le chiffre ne soient pas toujours au début
            return new string(password.ToString().ToCharArray().OrderBy(_ => RandomNumberGenerator.GetInt32(100)).ToArray());
        }
    }
}
