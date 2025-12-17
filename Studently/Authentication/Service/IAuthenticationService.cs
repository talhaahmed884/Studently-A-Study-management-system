using Studently.User.DTO;

namespace Studently.Authentication.Service;

public interface IAuthenticationService
{
    Task<UserDTO> SignUpAsync(SignUpRequestDTO request);

    Task<bool> LoginAsync(LoginRequestDTO request);

    Task<UserDTO?> GetUserByEmailAsync(string email);
}
