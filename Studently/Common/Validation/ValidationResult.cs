namespace Studently.Common.Validation;

/// <summary>
/// Validation result using Builder Pattern
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; }
    public IReadOnlyList<string> Errors { get; }

    private ValidationResult(List<string> errors)
    {
        Errors = errors.AsReadOnly();
        IsValid = errors.Count == 0;
    }

    public static ValidationResultBuilder Builder() => new();

    public static ValidationResult Valid() => new(new List<string>());

    public static ValidationResult Invalid(string error) => new(new List<string> { error });

    public string? GetFirstError() => Errors.Count > 0 ? Errors[0] : null;

    public class ValidationResultBuilder
    {
        private readonly List<string> _errors = new();

        public ValidationResultBuilder AddError(string error)
        {
            _errors.Add(error);
            return this;
        }

        public ValidationResult Build() => new(_errors);
    }
}
