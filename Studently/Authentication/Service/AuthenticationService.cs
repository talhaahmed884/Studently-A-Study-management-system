using Studently.User.DTO;
using Studently.User.Entity;
using Studently.User.Service;
using Studently.UserCredential.Service;

namespace Studently.Authentication.Service;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserService _userService;
    private readonly IUserCredentialService _credentialService;

    public AuthenticationService(
        IUserService userService,
        IUserCredentialService credentialService)
    {
        _userService = userService;
        _credentialService = credentialService;
    }

    public async Task<UserDTO> SignUpAsync(SignUpRequestDTO request)
    {
        // Validate request
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Sign-up request cannot be null");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new UserException(
                UserErrorCode.InvalidUserData,
                "Name is required");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new UserException(
                UserErrorCode.InvalidEmailFormat,
                "Email is required");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new UserCredential.Entity.UserCredentialException(
                UserCredential.Entity.UserCredentialErrorCode.PasswordRequired);
        }

        Studently.User.Entity.User? createdUser = null;

        try
        {
            // Step 1: Create user without credentials
            createdUser = await _userService.CreateUserWithoutCredentialAsync(
                request.Name,
                request.Email);

            // Step 2: Create credentials for the user
            await _credentialService.CreateCredentialAsync(
                createdUser.Id,
                request.Password);

            // Step 3: Return user DTO
            return UserDTO.FromEntity(createdUser);
        }
        catch
        {
            // Rollback: If credential creation fails, delete the created user
            if (createdUser != null)
            {
                try
                {
                    await _userService.DeleteUserAsync(createdUser.Id);
                }
                catch
                {
                    // Log error but don't mask the original exception
                    // In production, this should be logged properly
                }
            }

            // Re-throw the original exception
            throw;
        }
    }

    public async Task<bool> LoginAsync(LoginRequestDTO request)
    {
        // Validate request
        if (request == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return false;
        }

        try
        {
            // Verify password using credential service
            return await _credentialService.VerifyPasswordAsync(
                request.Email,
                request.Password);
        }
        catch
        {
            // On any exception, return false
            // This prevents information leakage about which step failed
            return false;
        }
    }

    public async Task<UserDTO?> GetUserByEmailAsync(string email)
    {
        // Validate email
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        try
        {
            // Delegate to user service
            return await _userService.GetUserByEmailAsync(email);
        }
        catch
        {
            // Return null on any exception
            return null;
        }
    }
}
