namespace Studently.UserCredential.SQL;

public static class UserCredentialQueries
{
    public const string SelectAllColumns = "user_id, password_hash, algorithm, created_at, updated_at";
    public const string FindByUserId = @"
        SELECT user_id, password_hash, algorithm, created_at, updated_at
        FROM user_credentials
        WHERE user_id = @UserId";
    public const string ExistsByUserId = @"
        SELECT COUNT(1)
        FROM user_credentials
        WHERE user_id = @UserId";
    public const string Insert = @"
        INSERT INTO user_credentials (user_id, password_hash, algorithm, created_at, updated_at)
        VALUES (@UserId, @PasswordHash, @Algorithm, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET())";
    public const string Update = @"
        UPDATE user_credentials
        SET password_hash = @PasswordHash,
            algorithm = @Algorithm,
            updated_at = SYSDATETIMEOFFSET()
        WHERE user_id = @UserId";
    public const string DeleteByUserId = @"
        DELETE FROM user_credentials
        WHERE user_id = @UserId";
    public const string Count = @"
        SELECT COUNT(*)
        FROM user_credentials";
    public const string FindByAlgorithm = @"
        SELECT user_id, password_hash, algorithm, created_at, updated_at
        FROM user_credentials
        WHERE algorithm = @Algorithm
        ORDER BY created_at DESC";
    public const string FindUserWithCredentials = @"
        SELECT
            u.id, u.name, u.email, u.created_at AS user_created_at, u.updated_at AS user_updated_at,
            uc.user_id, uc.password_hash, uc.algorithm, uc.created_at AS credential_created_at, uc.updated_at AS credential_updated_at
        FROM users u
        INNER JOIN user_credentials uc ON u.id = uc.user_id
        WHERE u.id = @UserId";
}
