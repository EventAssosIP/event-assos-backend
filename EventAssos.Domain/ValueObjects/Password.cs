using System.Text.RegularExpressions;

namespace EventAssos.Domain.ValueObjects;

public sealed record Password
{
    private static readonly Regex Regex =
        new(@"^(?=.*[A-Z])(?=.*\d).{8,}$", RegexOptions.Compiled);

    public string Value { get; }

    private Password(string value) => Value = value;

    public static Password Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Password cannot be empty", nameof(value));

        if (!Regex.IsMatch(value))
            throw new ArgumentException("Password must contain at least 8 characters, one uppercase letter and one number", nameof(value));

        return new Password(value);
    }
}
