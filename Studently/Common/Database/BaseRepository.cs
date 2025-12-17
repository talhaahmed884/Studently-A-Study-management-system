using System.Data;

namespace Studently.Common.Database;

public abstract class BaseRepository
{
    protected readonly IDbConnectionFactory ConnectionFactory;

    protected BaseRepository(IDbConnectionFactory connectionFactory)
    {
        ConnectionFactory = connectionFactory;
    }

    protected IDbConnection CreateConnection()
    {
        return ConnectionFactory.CreateConnection();
    }

    protected async Task<TResult> ExecuteWithConnectionAsync<TResult>(
        Func<IDbConnection, Task<TResult>> operation)
    {
        using var connection = CreateConnection();
        return await operation(connection);
    }

    protected async Task ExecuteWithConnectionAsync(
        Func<IDbConnection, Task> operation)
    {
        using var connection = CreateConnection();
        await operation(connection);
    }

    protected async Task<TResult> ExecuteInTransactionAsync<TResult>(
        Func<IDbConnection, IDbTransaction, Task<TResult>> operation)
    {
        using var connection = CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            var result = await operation(connection, transaction);
            transaction.Commit();
            return result;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    protected async Task ExecuteInTransactionAsync(
        Func<IDbConnection, IDbTransaction, Task> operation)
    {
        using var connection = CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            await operation(connection, transaction);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
