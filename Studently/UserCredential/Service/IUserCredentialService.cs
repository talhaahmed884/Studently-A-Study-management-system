namespace Studently.UserCredential.Service;

public interface IUserCredentialService
{
    Task CreateCredentialAsync(Guid userId, string password);

    Task SetPasswordAsync(string email, string newPassword);

    Task<bool> VerifyPasswordAsync(string email, string password);
}
