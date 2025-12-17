namespace Studently.User.Repository;

public interface IUserRepository
{
    Task<Entity.User> SaveAsync(Studently.User.Entity.User user);

    Task<Entity.User?> FindByIdAsync(Guid id);

    Task<Entity.User?> FindByEmailAsync(string email);

    Task<List<Entity.User>> FindAllAsync();

    Task DeleteAsync(Entity.User user);

    Task DeleteByIdAsync(Guid id);

    Task<bool> ExistsByEmailAsync(string email);

    Task<long> CountAsync();
}
