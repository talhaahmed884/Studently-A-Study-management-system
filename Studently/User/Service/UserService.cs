using Studently.User.DTO;
using Studently.User.Entity;
using Studently.User.Repository;

namespace Studently.User.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Entity.User> CreateUserWithoutCredentialAsync(string name, string email)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new UserException(
                UserErrorCode.InvalidUserData,
                "Name cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new UserException(
                UserErrorCode.InvalidEmailFormat,
                "Email cannot be null or empty");
        }

        // Check if user with email already exists
        var existingUser = await _userRepository.FindByEmailAsync(email);
        if (existingUser != null)
        {
            throw new UserException(
                UserErrorCode.EmailAlreadyInUse,
                email);
        }

        // Create user using builder
        var user = Entity.User.Builder()
            .WithName(name)
            .WithEmail(email)
            .Build();

        // Save to database
        await _userRepository.SaveAsync(user);

        return user;
    }

    public async Task<UserDTO?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.FindByIdAsync(id);
        return user != null ? UserDTO.FromEntity(user) : null;
    }

    public async Task<UserDTO?> GetUserByEmailAsync(string email)
    {
        // Validate email
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        var user = await _userRepository.FindByEmailAsync(email);
        return user != null ? UserDTO.FromEntity(user) : null;
    }

    public async Task UpdateUserAsync(Guid id, string name, string email)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new UserException(
                UserErrorCode.InvalidUserData,
                "Name cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new UserException(
                UserErrorCode.InvalidEmailFormat,
                "Email cannot be null or empty");
        }

        // Check if user exists
        var existingUser = await _userRepository.FindByIdAsync(id);
        if (existingUser == null)
        {
            throw new UserException(
                UserErrorCode.UserNotFound,
                "id", id.ToString());
        }

        // Check if new email is already in use by another user
        if (existingUser.Email != email)
        {
            var userWithEmail = await _userRepository.FindByEmailAsync(email);
            if (userWithEmail != null && userWithEmail.Id != id)
            {
                throw new UserException(
                    UserErrorCode.EmailAlreadyInUse,
                    email);
            }
        }

        // Rebuild user with updated information
        // Note: This creates a new user object with the same ID
        var updatedUser = Entity.User.Builder()
            .WithName(name)
            .WithEmail(email)
            .Build();

        // Need to set the ID using reflection since it's internal
        var idProp = typeof(Entity.User).GetProperty("Id",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        idProp?.SetValue(updatedUser, id);

        // Save to database (will update existing user)
        await _userRepository.SaveAsync(updatedUser);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        // Check if user exists
        var user = await _userRepository.FindByIdAsync(id);
        if (user == null)
        {
            throw new UserException(
                UserErrorCode.UserNotFound,
                "id", id.ToString());
        }

        // Delete user (cascade will delete credentials if configured)
        await _userRepository.DeleteByIdAsync(id);
    }
}
