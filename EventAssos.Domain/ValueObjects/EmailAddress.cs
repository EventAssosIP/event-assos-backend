using System.Text.RegularExpressions;

namespace EventAssos.Domain.ValueObjects
{
    public struct EmailAddress
    {
        private readonly Regex EmailRegex =
            new(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$ ^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Singleline);
        public string Value { get; }

        public EmailAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !EmailRegex.IsMatch(value))
                throw new ArgumentNullException("Unvalid email address");

            Value = value;
        }
    }
}
