namespace Studently.Common.Validation;

public class UserNameValidator
{
    public ValidationResult Validate(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return ValidationResult.Invalid("Name cannot be null or empty");
        }

        if (name.Length < 2)
        {
            return ValidationResult.Invalid("Name must be at least 2 characters long");
        }

        if (name.Length > 100)
        {
            return ValidationResult.Invalid("Name must not exceed 100 characters");
        }

        return ValidationResult.Valid();
    }
}
