namespace Studently.User.Entity;

public class User
{
    private Guid Id { get; set; }

    private string Name {get; set;}

    private string Email{get; set;}

    private DateTime CreatedAt { get; set; }

    private DateTime UpdatedAt { get; set; }

    protected User()
    {
        Id = Guid.NewGuid();
    }

    internal User(UserBuilder builder)
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
