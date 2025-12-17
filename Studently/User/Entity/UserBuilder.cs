namespace Studently.User.Entity;

public class UserBuilder
{
    internal string Name { get; set; } = string.Empty;
    internal string Email { get; set; } = string.Empty;

    public User Build()
    {
        return new User(this);
    }
}
