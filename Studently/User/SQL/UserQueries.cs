namespace Studently.User.SQL;

public static class UserQueries
{
    public const string SelectAllColumns = "id, name, email, created_at, updated_at";
    public const string FindById = @"
        SELECT id, name, email, created_at, updated_at
        FROM users
        WHERE id = @Id";
    public const string FindByEmail = @"
        SELECT id, name, email, created_at, updated_at
        FROM users
        WHERE email = @Email";
    public const string FindAll = @"
        SELECT id, name, email, created_at, updated_at
        FROM users
        ORDER BY created_at DESC";
    public const string ExistsById = @"
        SELECT COUNT(1)
        FROM users
        WHERE id = @Id";
    public const string ExistsByEmail = @"
        SELECT COUNT(1)
        FROM users
        WHERE email = @Email";
    public const string Insert = @"
        INSERT INTO users (id, name, email, created_at, updated_at)
        VALUES (@Id, @Name, @Email, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET())";
    public const string Update = @"
        UPDATE users
        SET name = @Name,
            email = @Email,
            updated_at = SYSDATETIMEOFFSET()
        WHERE id = @Id";
    public const string DeleteById = @"
        DELETE FROM users
        WHERE id = @Id";
    public const string Count = @"
        SELECT COUNT(*)
        FROM users";
    public const string FindByDateRange = @"
        SELECT id, name, email, created_at, updated_at
        FROM users
        WHERE created_at >= @StartDate AND created_at <= @EndDate
        ORDER BY created_at DESC";
    public const string FindByNameLike = @"
        SELECT id, name, email, created_at, updated_at
        FROM users
        WHERE name LIKE @Pattern
        ORDER BY name";
    public const string FindPaginated = @"
        SELECT id, name, email, created_at, updated_at
        FROM users
        ORDER BY created_at DESC
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY";
}
