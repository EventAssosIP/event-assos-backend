using System.Text.RegularExpressions;

namespace EventAssos.Domain.ValueObjects
{
    public struct Password
    {
        private static readonly Regex PasswordRegex =
            new(@"^(?=.*[A-Z])(?=.*\d).{8,}$", RegexOptions.Compiled);

        public string Value { get; }

        public Password(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Password cannot be empty");

            if (!PasswordRegex.IsMatch(value))
                throw new ArgumentException("Password must contain at least 8 characters, one uppercase letter and one number");

            Value = value;
        }
    }
}
