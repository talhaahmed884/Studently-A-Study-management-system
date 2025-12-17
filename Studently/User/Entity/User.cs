namespace Studently.User.Entity;

public class User
{
    internal Guid Id { get; private set; }
    internal string Name { get; private set; }
    internal string Email { get; private set; }
    internal DateTime CreatedAt { get; private set; }
    internal DateTime UpdatedAt { get; private set; }

    protected User()
    {
        Id = Guid.NewGuid();
        Name = string.Empty;
        Email = string.Empty;
    }

    public User(UserBuilder builder)
    {
        Id = Guid.NewGuid();
        Name = builder.Name;
        Email = builder.Email;
    }

    public static UserBuilder Builder() => new();

    public override bool Equals(object? obj)
    {
        if (obj is not User other)
            return false;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
