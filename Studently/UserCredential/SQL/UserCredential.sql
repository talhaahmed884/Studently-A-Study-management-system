-- ============================================================================
-- USER MANAGEMENT SYSTEM - USER CREDENTIALS SCHEMA
-- Database: SQL Server
-- ============================================================================

-- Use the Studently database
USE Studently;
GO

-- ============================================================================
-- DROP EXISTING OBJECTS (for clean setup)
-- ============================================================================

-- Drop triggers first
IF OBJECT_ID('dbo.trg_user_credentials_update_timestamp', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_user_credentials_update_timestamp;
GO

-- Drop table if it exists
IF OBJECT_ID('dbo.user_credentials', 'U') IS NOT NULL
    DROP TABLE dbo.user_credentials;
GO

-- ============================================================================
-- USER_CREDENTIALS TABLE
-- ============================================================================

CREATE TABLE user_credentials
(
    user_id       UNIQUEIDENTIFIER PRIMARY KEY,
    password_hash NVARCHAR(512)    NOT NULL,
    algorithm     NVARCHAR(50)     NOT NULL DEFAULT 'SHA-512',
    created_at    DATETIMEOFFSET   NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    updated_at    DATETIMEOFFSET   NOT NULL DEFAULT SYSDATETIMEOFFSET(),

    -- Foreign Key Constraint
    CONSTRAINT fk_user_credentials_user
        FOREIGN KEY (user_id)
            REFERENCES users (id)
            ON DELETE CASCADE
            ON UPDATE CASCADE
);
GO

-- ============================================================================
-- TRIGGERS FOR UPDATED_AT TIMESTAMP
-- ============================================================================

-- Trigger to automatically update the updated_at timestamp
CREATE TRIGGER trg_user_credentials_update_timestamp
ON user_credentials
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE user_credentials
    SET updated_at = SYSDATETIMEOFFSET()
    FROM user_credentials uc
    INNER JOIN inserted i ON uc.user_id = i.user_id;
END;
GO

-- ============================================================================
-- EXTENDED PROPERTIES (Documentation)
-- ============================================================================

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Stores user authentication credentials',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'user_credentials';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Foreign key reference to users table (one-to-one relationship)',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'user_credentials',
    @level2type = N'COLUMN', @level2name = N'user_id';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Hashed password with salt (Base64 encoded)',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'user_credentials',
    @level2type = N'COLUMN', @level2name = N'password_hash';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Hashing algorithm used (default: SHA-512)',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'user_credentials',
    @level2type = N'COLUMN', @level2name = N'algorithm';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Timestamp when the credentials were created',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'user_credentials',
    @level2type = N'COLUMN', @level2name = N'created_at';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Timestamp when the credentials were last updated',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'user_credentials',
    @level2type = N'COLUMN', @level2name = N'updated_at';
GO

-- ============================================================================
-- SAMPLE DATA (For Testing)
-- ============================================================================

-- Insert sample credentials (Note: These are example hashes, not real passwords)
-- INSERT INTO user_credentials (user_id, password_hash, algorithm, created_at, updated_at)
-- VALUES
--     (CAST('550e8400-e29b-41d4-a716-446655440000' AS UNIQUEIDENTIFIER),
--      'aGFzaGVkX3Bhc3N3b3JkXzEyMzQ1Njc4OTBhYmNkZWZnaGlqa2xtbm9wcXJzdHV2d3h5eg==',
--      'SHA-512',
--      SYSDATETIMEOFFSET(),
--      SYSDATETIMEOFFSET()),
--     (CAST('550e8400-e29b-41d4-a716-446655440001' AS UNIQUEIDENTIFIER),
--      'YW5vdGhlcl9oYXNoZWRfcGFzc3dvcmRfZm9yX3Rlc3Rpbmdfb25seQ==',
--      'SHA-512',
--      SYSDATETIMEOFFSET(),
--      SYSDATETIMEOFFSET()),
--     (CAST('550e8400-e29b-41d4-a716-446655440002' AS UNIQUEIDENTIFIER),
--      'dGhpcmRfdXNlcl9oYXNoZWRfcGFzc3dvcmRfZXhhbXBsZQ==',
--      'SHA-512',
--      SYSDATETIMEOFFSET(),
--      SYSDATETIMEOFFSET());
-- GO

-- ============================================================================
-- COMMON QUERIES (SQL Server Syntax)
-- ============================================================================

-- 1. Authenticate user - get user with credentials
-- SELECT u.id, u.name, u.email, uc.password_hash, uc.algorithm
-- FROM users u
-- INNER JOIN user_credentials uc ON u.id = uc.user_id
-- WHERE u.email = 'john.doe@example.com';

-- 2. Get all users with their credential info
-- SELECT u.id, u.name, u.email, u.created_at, uc.algorithm, uc.created_at AS credential_created
-- FROM users u
-- LEFT JOIN user_credentials uc ON u.id = uc.user_id
-- ORDER BY u.created_at DESC;

-- 3. Update password (hash should be generated in application)
-- UPDATE user_credentials
-- SET password_hash = 'new_hashed_password_here'
-- WHERE user_id = CAST('550e8400-e29b-41d4-a716-446655440000' AS UNIQUEIDENTIFIER);

-- 4. Delete user (cascade will delete credentials)
-- DELETE FROM users WHERE id = CAST('550e8400-e29b-41d4-a716-446655440000' AS UNIQUEIDENTIFIER);

-- 5. Check if user has credentials
-- SELECT CASE WHEN EXISTS(
--     SELECT 1 FROM user_credentials
--     WHERE user_id = CAST('550e8400-e29b-41d4-a716-446655440000' AS UNIQUEIDENTIFIER)
-- ) THEN 1 ELSE 0 END AS has_credentials;

-- 6. Find users without credentials
-- SELECT u.id, u.name, u.email
-- FROM users u
-- LEFT JOIN user_credentials uc ON u.id = uc.user_id
-- WHERE uc.user_id IS NULL;

-- 7. Count credentials by algorithm
-- SELECT algorithm, COUNT(*) AS count
-- FROM user_credentials
-- GROUP BY algorithm;

-- ============================================================================
-- PERFORMANCE ANALYSIS QUERIES
-- ============================================================================

-- Check index usage statistics
-- SELECT
--     OBJECT_NAME(ius.object_id) AS table_name,
--     i.name AS index_name,
--     ius.user_seeks,
--     ius.user_scans,
--     ius.user_lookups,
--     ius.user_updates
-- FROM sys.dm_db_index_usage_stats ius
-- INNER JOIN sys.indexes i ON ius.object_id = i.object_id AND ius.index_id = i.index_id
-- WHERE OBJECT_NAME(ius.object_id) IN ('users', 'user_credentials')
-- ORDER BY ius.user_seeks + ius.user_scans + ius.user_lookups DESC;

-- Check table statistics
-- SELECT
--     OBJECT_NAME(p.object_id) AS table_name,
--     SUM(p.rows) AS row_count,
--     SUM(a.total_pages) * 8 AS total_space_kb,
--     SUM(a.used_pages) * 8 AS used_space_kb
-- FROM sys.partitions p
-- INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
-- WHERE OBJECT_NAME(p.object_id) IN ('users', 'user_credentials')
-- GROUP BY OBJECT_NAME(p.object_id);

-- Analyze query performance with execution plan
-- SET STATISTICS IO ON;
-- SET STATISTICS TIME ON;
-- SELECT u.id, u.name, u.email, uc.password_hash, uc.algorithm
-- FROM users u
-- INNER JOIN user_credentials uc ON u.id = uc.user_id
-- WHERE u.email = 'john.doe@example.com';
-- SET STATISTICS IO OFF;
-- SET STATISTICS TIME OFF;

-- Check foreign key relationships
-- SELECT
--     fk.name AS foreign_key_name,
--     OBJECT_NAME(fk.parent_object_id) AS table_name,
--     COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS column_name,
--     OBJECT_NAME(fk.referenced_object_id) AS referenced_table,
--     COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS referenced_column
-- FROM sys.foreign_keys fk
-- INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
-- WHERE OBJECT_NAME(fk.parent_object_id) = 'user_credentials';

-- ============================================================================
-- MAINTENANCE TASKS
-- ============================================================================

-- Update statistics
-- UPDATE STATISTICS user_credentials WITH FULLSCAN;

-- Rebuild indexes
-- ALTER INDEX ALL ON user_credentials REBUILD;

-- Reorganize indexes (for moderate fragmentation)
-- ALTER INDEX ALL ON user_credentials REORGANIZE;

-- Check index fragmentation
-- SELECT
--     OBJECT_NAME(ips.object_id) AS table_name,
--     i.name AS index_name,
--     ips.index_type_desc,
--     ips.avg_fragmentation_in_percent,
--     ips.page_count
-- FROM sys.dm_db_index_physical_stats(DB_ID(), OBJECT_ID('user_credentials'), NULL, NULL, 'DETAILED') ips
-- INNER JOIN sys.indexes i ON ips.object_id = i.object_id AND ips.index_id = i.index_id
-- ORDER BY ips.avg_fragmentation_in_percent DESC;

-- ============================================================================
-- SECURITY BEST PRACTICES
-- ============================================================================

-- Grant permissions to application user
-- GRANT SELECT, INSERT, UPDATE, DELETE ON user_credentials TO app_user;
-- GO

-- Deny direct access to password_hash column for reporting users (example)
-- DENY SELECT ON user_credentials (password_hash) TO reporting_user;
-- GO

-- Create a view for safe credential checking (without exposing hash)
-- CREATE VIEW vw_user_login AS
-- SELECT u.id, u.email, uc.algorithm, uc.password_hash
-- FROM users u
-- INNER JOIN user_credentials uc ON u.id = uc.user_id;
-- GO

-- ============================================================================
-- BACKUP AND RESTORE
-- ============================================================================

-- Backup database (includes all tables):
-- BACKUP DATABASE Studently TO DISK = 'C:\Backups\Studently.bak' WITH INIT;

-- Restore database:
-- RESTORE DATABASE Studently FROM DISK = 'C:\Backups\Studently.bak' WITH REPLACE;

-- ============================================================================
-- VERIFICATION QUERIES
-- ============================================================================

-- Verify table creation
-- SELECT TABLE_NAME
-- FROM INFORMATION_SCHEMA.TABLES
-- WHERE TABLE_SCHEMA = 'dbo' AND TABLE_TYPE = 'BASE TABLE';

-- Verify foreign key constraints
-- SELECT
--     fk.name AS constraint_name,
--     OBJECT_NAME(fk.parent_object_id) AS table_name,
--     OBJECT_NAME(fk.referenced_object_id) AS referenced_table
-- FROM sys.foreign_keys fk
-- WHERE OBJECT_NAME(fk.parent_object_id) = 'user_credentials';

-- Verify sample data
-- SELECT uc.user_id, u.name, u.email, uc.algorithm
-- FROM user_credentials uc
-- INNER JOIN users u ON uc.user_id = u.id;

-- Test cascade delete (WARNING: This will delete data!)
-- BEGIN TRANSACTION;
-- DELETE FROM users WHERE id = CAST('550e8400-e29b-41d4-a716-446655440002' AS UNIQUEIDENTIFIER);
-- SELECT * FROM user_credentials WHERE user_id = CAST('550e8400-e29b-41d4-a716-446655440002' AS UNIQUEIDENTIFIER);
-- ROLLBACK TRANSACTION; -- Remove this line to commit the delete
