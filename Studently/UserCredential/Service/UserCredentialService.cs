using Studently.User.Entity;
using Studently.User.Repository;
using Studently.UserCredential.Entity;
using Studently.UserCredential.Repository;

namespace Studently.UserCredential.Service;

public class UserCredentialService : IUserCredentialService
{
    private readonly IUserCredentialRepository _credentialRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashingStrategy _hashingStrategy;

    public UserCredentialService(
        IUserCredentialRepository credentialRepository,
        IUserRepository userRepository,
        IPasswordHashingStrategy hashingStrategy)
    {
        _credentialRepository = credentialRepository;
        _userRepository = userRepository;
        _hashingStrategy = hashingStrategy;
    }

    public async Task CreateCredentialAsync(Guid userId, string password)
    {
        // Validate password is not empty
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new UserCredentialException(
                UserCredentialErrorCode.InvalidPassword,
                "Password cannot be null or empty");
        }

        // Check if user exists
        var user = await _userRepository.FindByIdAsync(userId);
        if (user == null)
        {
            throw new UserException(
                UserErrorCode.UserNotFound,
                "id", userId.ToString());
        }

        // Check if credential already exists
        var existingCredential = await _credentialRepository.FindByUserIdAsync(userId);
        if (existingCredential != null)
        {
            throw new UserCredentialException(
                UserCredentialErrorCode.CredentialAlreadyExists,
                userId.ToString());
        }

        // Hash the password
        string hashedPassword = _hashingStrategy.Hash(password);

        // Create credential using builder
        var credential = Entity.UserCredential.Builder()
            .SetUserId(userId)
            .SetPasswordHash(hashedPassword)
            .SetAlgorithm(_hashingStrategy.AlgorithmName)
            .Build();

        // Save to database
        await _credentialRepository.SaveAsync(credential);
    }

    public async Task SetPasswordAsync(string email, string newPassword)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new UserException(
                UserErrorCode.InvalidEmailFormat,
                "Email cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(newPassword))
        {
            throw new UserCredentialException(
                UserCredentialErrorCode.InvalidPassword,
                "Password cannot be null or empty");
        }

        // Find user by email
        var user = await _userRepository.FindByEmailAsync(email);
        if (user == null)
        {
            throw new UserException(
                UserErrorCode.UserNotFound,
                "email", email);
        }

        // Hash the new password
        string hashedPassword = _hashingStrategy.Hash(newPassword);

        // Check if credential exists
        var credential = await _credentialRepository.FindByUserIdAsync(user.Id);
        if (credential == null)
        {
            throw new UserCredentialException(
                UserCredentialErrorCode.CredentialNotFound, email);
        }
        else
        {
            // Update existing credential (rebuild with new password)
            credential = Entity.UserCredential.Builder()
                .SetUserId(user.Id)
                .SetPasswordHash(hashedPassword)
                .SetAlgorithm(_hashingStrategy.AlgorithmName)
                .Build();
        }

        // Save to database
        await _credentialRepository.SaveAsync(credential);
    }

    public async Task<bool> VerifyPasswordAsync(string email, string password)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        try
        {
            // Find user by email
            var user = await _userRepository.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            // Find credential
            var credential = await _credentialRepository.FindByUserIdAsync(user.Id);
            if (credential == null)
            {
                return false;
            }

            // Verify password using hashing strategy
            return _hashingStrategy.Verify(password, credential.PasswordHash);
        }
        catch
        {
            // On any exception during verification, return false
            // This prevents information leakage about which step failed
            return false;
        }
    }
}
