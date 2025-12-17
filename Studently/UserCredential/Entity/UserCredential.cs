namespace Studently.UserCredential.Entity;

public class UserCredential
{
    internal Guid UserId { get; private set; } = Guid.Empty;
    internal string PasswordHash { get; private set; } = String.Empty;
    internal string Algorithm { get; private set; } = String.Empty;

    protected UserCredential() { }

    public UserCredential(UserCredentialBuilder builder)
    {
        UserId = builder.UserId;
        PasswordHash = builder.PasswordHash;
        Algorithm = builder.Algorithm;
    }

    public static UserCredentialBuilder Builder() => new();

    public override bool Equals(object? obj)
    {
        if (obj is not UserCredential other)
            return false;

        return UserId.Equals(other.UserId);
    }

    public override int GetHashCode()
    {
        return UserId.GetHashCode();
    }
}