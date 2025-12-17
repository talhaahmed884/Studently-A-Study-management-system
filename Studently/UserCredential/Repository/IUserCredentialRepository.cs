namespace Studently.UserCredential.Repository;

public interface IUserCredentialRepository
{
    Task<Entity.UserCredential> SaveAsync(Entity.UserCredential credential);

    Task<Entity.UserCredential?> FindByUserIdAsync(Guid userId);

    Task DeleteAsync(Entity.UserCredential credential);

    Task DeleteByUserIdAsync(Guid userId);

    Task<bool> ExistsByUserIdAsync(Guid userId);

    Task<long> CountAsync();
}
