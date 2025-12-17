namespace Studently.UserCredential.Entity;

public class UserCredentialBuilder
{
    internal Guid UserId { get; private set; } = Guid.Empty;
    internal string PasswordHash { get; private set; } = string.Empty;
    internal string Algorithm { get; private set; } = string.Empty;

    public UserCredentialBuilder SetUserId(Guid userId)
    {
        UserId = userId;
        return this;
    }

    public UserCredentialBuilder SetPasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
        return this;
    }

    public UserCredentialBuilder SetAlgorithm(string algorithm)
    {
        Algorithm = algorithm;
        return this;
    }

    public UserCredential Build()
    {
        return new UserCredential(this);
    }
}
