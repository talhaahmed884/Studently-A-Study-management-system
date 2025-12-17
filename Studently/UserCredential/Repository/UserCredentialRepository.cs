using Dapper;
using Studently.Common.Database;
using Studently.UserCredential.SQL;

namespace Studently.UserCredential.Repository;

public class UserCredentialRepository : BaseRepository, IUserCredentialRepository
{
    public UserCredentialRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    private Entity.UserCredential MapToUserCredential(dynamic row)
    {
        var credential = (Entity.UserCredential)Activator.CreateInstance(typeof(Entity.UserCredential), true)!;

        // Use reflection to set internal properties
        var userIdProp = typeof(Entity.UserCredential).GetProperty("UserId",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var passwordHashProp = typeof(Entity.UserCredential).GetProperty("PasswordHash",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var algorithmProp = typeof(Entity.UserCredential).GetProperty("Algorithm",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        userIdProp?.SetValue(credential, (Guid)row.user_id);
        passwordHashProp?.SetValue(credential, (string)row.password_hash);
        algorithmProp?.SetValue(credential, (string)row.algorithm);

        return credential;
    }

    public async Task<Entity.UserCredential> SaveAsync(Entity.UserCredential credential)
    {
        return await ExecuteWithConnectionAsync(async connection =>
        {
            var exists = await connection.ExecuteScalarAsync<bool>(
                UserCredentialQueries.ExistsByUserId,
                new { UserId = credential.UserId });

            if (exists)
            {
                await connection.ExecuteAsync(UserCredentialQueries.Update, new
                {
                    UserId = credential.UserId,
                    PasswordHash = credential.PasswordHash,
                    Algorithm = credential.Algorithm
                });
            }
            else
            {
                await connection.ExecuteAsync(UserCredentialQueries.Insert, new
                {
                    UserId = credential.UserId,
                    PasswordHash = credential.PasswordHash,
                    Algorithm = credential.Algorithm
                });
            }

            return credential;
        });
    }

    public async Task<Entity.UserCredential?> FindByUserIdAsync(Guid userId)
    {
        return await ExecuteWithConnectionAsync(async connection =>
        {
            var row = await connection.QuerySingleOrDefaultAsync(
                UserCredentialQueries.FindByUserId,
                new { UserId = userId });

            return row != null ? MapToUserCredential(row) : null;
        });
    }

    public async Task DeleteAsync(Entity.UserCredential credential)
    {
        await DeleteByUserIdAsync(credential.UserId);
    }

    public async Task DeleteByUserIdAsync(Guid userId)
    {
        await ExecuteWithConnectionAsync(async connection =>
        {
            await connection.ExecuteAsync(UserCredentialQueries.DeleteByUserId, new { UserId = userId });
        });
    }

    public async Task<bool> ExistsByUserIdAsync(Guid userId)
    {
        return await ExecuteWithConnectionAsync(async connection =>
        {
            var count = await connection.ExecuteScalarAsync<int>(
                UserCredentialQueries.ExistsByUserId,
                new { UserId = userId });

            return count > 0;
        });
    }

    public async Task<long> CountAsync()
    {
        return await ExecuteWithConnectionAsync(async connection =>
        {
            return await connection.ExecuteScalarAsync<long>(UserCredentialQueries.Count);
        });
    }
}
