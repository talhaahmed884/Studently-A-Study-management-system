namespace Studently.User.DTO;

public class UserDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public static UserDTO FromEntity(Entity.User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User entity cannot be null");
        }

        return new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    public static List<UserDTO> FromEntities(IEnumerable<Entity.User> users)
    {
        if (users == null)
        {
            throw new ArgumentNullException(nameof(users), "Users collection cannot be null");
        }

        return users.Select(FromEntity).ToList();
    }
}
