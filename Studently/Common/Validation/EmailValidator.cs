using System.Text.RegularExpressions;

namespace Studently.Common.Validation;

public class EmailValidator
{
    private static readonly Regex EmailRegex = GeneratedRegexAttribute(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public ValidationResult Validate(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return ValidationResult.Invalid("Email cannot be null or empty");
        }

        if (!EmailRegex.IsMatch(email))
        {
            return ValidationResult.Invalid($"Invalid email format: {email}");
        }

        return ValidationResult.Valid();
    }
}
