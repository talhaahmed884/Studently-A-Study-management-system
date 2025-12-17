namespace Studently.User.Entity;

public class UserBuilder
{
    internal string Name { get; private set; } = string.Empty;
    internal string Email { get; private set; } = string.Empty;

    public UserBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        Email = email;
        return this;
    }

    public User Build()
    {
        return new User(this);
    }
}
