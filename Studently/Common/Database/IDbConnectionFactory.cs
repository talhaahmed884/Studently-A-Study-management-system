using System.Data;

namespace Studently.Common.Database;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
