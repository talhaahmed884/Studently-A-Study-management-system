using Dapper;
using Studently.Common.Database;
using Studently.User.Entity;
using Studently.User.SQL;

namespace Studently.User.Repository;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    private Entity.User MapToUser(dynamic row)
    {
        var user = (Entity.User)Activator.CreateInstance(typeof(Entity.User), true)!;

        // Use reflection to set internal properties
        var idProp = typeof(Entity.User).GetProperty("Id", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var nameProp = typeof(Entity.User).GetProperty("Name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var emailProp = typeof(Entity.User).GetProperty("Email", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var createdAtProp = typeof(Entity.User).GetProperty("CreatedAt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var updatedAtProp = typeof(Entity.User).GetProperty("UpdatedAt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        idProp?.SetValue(user, (Guid)row.id);
        nameProp?.SetValue(user, (string)row.name);
        emailProp?.SetValue(user, (string)row.email);
        createdAtProp?.SetValue(user, ((DateTimeOffset)row.created_at).DateTime);
        updatedAtProp?.SetValue(user, ((DateTimeOffset)row.updated_at).DateTime);

        return user;
    }

    public async Task<Entity.User> SaveAsync(Entity.User user)
    {
        return await ExecuteWithConnectionAsync(async connection =>
        {
            var exists = await connection.ExecuteScalarAsync<bool>(
                UserQueries.ExistsById,
                new { Id = user.Id });

            if (exists)
            {
                await connection.ExecuteAsync(UserQueries.Update, new
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                });
            }
            else
            {
                await connection.ExecuteAsync(UserQueries.Insert, new
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                });
            }

            return user;
        });
    }

    public async Task<Entity.User?> FindByIdAsync(Guid id)
    {
        return await ExecuteWithConnectionAsync(async connection =>
        {
            var row = await connection.QuerySingleOrDefaultAsync(
                UserQueries.FindById,
                new { Id = id });

            return row != null ? MapToUser(row) : null;
        });
    }

    public async Task<Entity.User?> FindByEmailAsync(string email)
    {
        return await ExecuteWithConnectionAsync(async connection =>
        {
            var row = await connection.QuerySingleOrDefaultAsync(
                UserQueries.FindByEmail,
                new { Email = email });

            return row != null ? MapToUser(row) : null;
        });
    }

    public async Task<List<Entity.User>> FindAllAsync()
    {
        return await ExecuteWithConnectionAsync(async connection =>
        {
            var rows = await connection.QueryAsync(UserQueries.FindAll);

            var users = new List<Entity.User>();
            foreach (var row in rows)
            {
                users.Add(MapToUser(row));
            }

            return users;
        });
    }

    public async Task DeleteAsync(Entity.User user)
    {
        await DeleteByIdAsync(user.Id);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await ExecuteWithConnectionAsync(async connection =>
        {
            await connection.ExecuteAsync(UserQueries.DeleteById, new { Id = id });
        });
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await ExecuteWithConnectionAsync(async connection =>
        {
            var count = await connection.ExecuteScalarAsync<int>(
                UserQueries.ExistsByEmail,
                new { Email = email });

            return count > 0;
        });
    }

    public async Task<long> CountAsync()
    {
        return await ExecuteWithConnectionAsync(async connection =>
        {
            return await connection.ExecuteScalarAsync<long>(UserQueries.Count);
        });
    }
}
