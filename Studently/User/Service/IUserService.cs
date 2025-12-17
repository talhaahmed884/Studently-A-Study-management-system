using Studently.User.DTO;

namespace Studently.User.Service;

public interface IUserService
{
    Task<Entity.User> CreateUserWithoutCredentialAsync(string name, string email);

    Task<UserDTO?> GetUserByIdAsync(Guid id);

    Task<UserDTO?> GetUserByEmailAsync(string email);

    Task UpdateUserAsync(Guid id, string name, string email);

    Task DeleteUserAsync(Guid id);
}
