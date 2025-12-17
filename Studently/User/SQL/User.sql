-- ============================================================================
-- USER MANAGEMENT SYSTEM - DATABASE SCHEMA
-- Database: SQL Server
-- ============================================================================

-- ============================================================================
-- DATABASE CREATION AND SELECTION
-- ============================================================================

-- Create database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Studently')
BEGIN
    CREATE DATABASE Studently;
END
GO

-- Use the Studently database
USE Studently;
GO

-- ============================================================================
-- DROP EXISTING OBJECTS (for clean setup)
-- ============================================================================

-- Drop triggers first
IF OBJECT_ID('dbo.trg_users_update_timestamp', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_users_update_timestamp;
GO

-- Drop tables if they exist
IF OBJECT_ID('dbo.user_credentials', 'U') IS NOT NULL
    DROP TABLE dbo.user_credentials;
GO

IF OBJECT_ID('dbo.users', 'U') IS NOT NULL
    DROP TABLE dbo.users;
GO

-- ============================================================================
-- USERS TABLE
-- ============================================================================

CREATE TABLE users
(
    id         UNIQUEIDENTIFIER PRIMARY KEY,
    name       NVARCHAR(255)    NOT NULL,
    email      NVARCHAR(255)    NOT NULL UNIQUE,
    created_at DATETIMEOFFSET   NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    updated_at DATETIMEOFFSET   NOT NULL DEFAULT SYSDATETIMEOFFSET()
);
GO

-- ============================================================================
-- INDEXES FOR PERFORMANCE
-- ============================================================================

-- Index on email for fast lookups during login
CREATE NONCLUSTERED INDEX idx_users_email
    ON users (email);
GO

-- Index on created_at for sorting/filtering users by registration date
CREATE NONCLUSTERED INDEX idx_users_created_at
    ON users (created_at DESC);
GO

-- ============================================================================
-- TRIGGERS FOR UPDATED_AT TIMESTAMP
-- ============================================================================

-- Trigger to automatically update the updated_at timestamp
CREATE TRIGGER trg_users_update_timestamp
ON users
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE users
    SET updated_at = SYSDATETIMEOFFSET()
    FROM users u
    INNER JOIN inserted i ON u.id = i.id;
END;
GO

-- ============================================================================
-- EXTENDED PROPERTIES (Documentation)
-- ============================================================================

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Stores user account information',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'users';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Unique identifier for the user (GUID)',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'users',
    @level2type = N'COLUMN', @level2name = N'id';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Full name of the user',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'users',
    @level2type = N'COLUMN', @level2name = N'name';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Email address (unique, used for login)',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'users',
    @level2type = N'COLUMN', @level2name = N'email';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Timestamp when the user account was created',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'users',
    @level2type = N'COLUMN', @level2name = N'created_at';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Timestamp when the user record was last updated',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'users',
    @level2type = N'COLUMN', @level2name = N'updated_at';
GO

-- ============================================================================
-- SAMPLE DATA (For Testing)
-- ============================================================================

-- Insert sample users
-- INSERT INTO users (id, name, email, created_at, updated_at)
-- VALUES
--     (CAST('550e8400-e29b-41d4-a716-446655440000' AS UNIQUEIDENTIFIER), 'John Doe', 'john.doe@example.com', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()),
--     (CAST('550e8400-e29b-41d4-a716-446655440001' AS UNIQUEIDENTIFIER), 'Jane Smith', 'jane.smith@example.com', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()),
--     (CAST('550e8400-e29b-41d4-a716-446655440002' AS UNIQUEIDENTIFIER), 'Bob Wilson', 'bob.wilson@example.com', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());
-- GO

-- Note: user_credentials table is referenced but not defined in the original schema
-- Uncomment the following if the user_credentials table exists:
/*
INSERT INTO user_credentials (user_id, password_hash, algorithm)
VALUES
    (CAST('550e8400-e29b-41d4-a716-446655440000' AS UNIQUEIDENTIFIER),
     'aGFzaGVkX3Bhc3N3b3JkXzEyMzQ1Njc4OTBhYmNkZWZnaGlqa2xtbm9wcXJzdHV2d3h5eg==',
     'SHA-512'),
    (CAST('550e8400-e29b-41d4-a716-446655440001' AS UNIQUEIDENTIFIER),
     'YW5vdGhlcl9oYXNoZWRfcGFzc3dvcmRfZm9yX3Rlc3Rpbmdfb25seQ==',
     'SHA-512'),
    (CAST('550e8400-e29b-41d4-a716-446655440002' AS UNIQUEIDENTIFIER),
     'dGhpcmRfdXNlcl9oYXNoZWRfcGFzc3dvcmRfZXhhbXBsZQ==',
     'SHA-512');
GO
*/

-- ============================================================================
-- COMMON QUERIES (SQL Server Syntax)
-- ============================================================================

-- 1. Find user by email (used in login)
-- SELECT * FROM users WHERE email = 'john.doe@example.com';

-- 2. Count total users
-- SELECT COUNT(*) FROM users;

-- 3. Find recently registered users (last 7 days)
-- SELECT id, name, email, created_at
-- FROM users
-- WHERE created_at > DATEADD(DAY, -7, SYSDATETIMEOFFSET())
-- ORDER BY created_at DESC;

-- 4. Update user information
-- UPDATE users
-- SET name = 'John Smith', email = 'john.smith@example.com'
-- WHERE id = CAST('550e8400-e29b-41d4-a716-446655440000' AS UNIQUEIDENTIFIER);

-- 5. Delete user (if foreign keys configured with CASCADE, they will delete credentials)
-- DELETE FROM users WHERE id = CAST('550e8400-e29b-41d4-a716-446655440000' AS UNIQUEIDENTIFIER);

-- 6. Check if email exists
-- SELECT CASE WHEN EXISTS(SELECT 1 FROM users WHERE email = 'test@example.com')
--        THEN 1 ELSE 0 END AS email_exists;

-- 7. Get user count by creation month
-- SELECT
--     DATEFROMPARTS(YEAR(created_at), MONTH(created_at), 1) AS month,
--     COUNT(*) AS user_count
-- FROM users
-- GROUP BY DATEFROMPARTS(YEAR(created_at), MONTH(created_at), 1)
-- ORDER BY month DESC;

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

-- Analyze query performance (Execution Plan)
-- SET STATISTICS IO ON;
-- SET STATISTICS TIME ON;
-- SELECT u.*
-- FROM users u
-- WHERE u.email = 'john.doe@example.com';
-- SET STATISTICS IO OFF;
-- SET STATISTICS TIME OFF;

-- ============================================================================
-- MAINTENANCE TASKS
-- ============================================================================

-- Update statistics (SQL Server equivalent of VACUUM ANALYZE)
-- UPDATE STATISTICS users WITH FULLSCAN;
-- UPDATE STATISTICS user_credentials WITH FULLSCAN;

-- Rebuild indexes (if fragmentation is high)
-- ALTER INDEX ALL ON users REBUILD;
-- ALTER INDEX ALL ON user_credentials REBUILD;

-- Reorganize indexes (for moderate fragmentation)
-- ALTER INDEX ALL ON users REORGANIZE;
-- ALTER INDEX ALL ON user_credentials REORGANIZE;

-- Check index fragmentation
-- SELECT
--     OBJECT_NAME(ips.object_id) AS table_name,
--     i.name AS index_name,
--     ips.index_type_desc,
--     ips.avg_fragmentation_in_percent,
--     ips.page_count
-- FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'DETAILED') ips
-- INNER JOIN sys.indexes i ON ips.object_id = i.object_id AND ips.index_id = i.index_id
-- WHERE OBJECT_NAME(ips.object_id) IN ('users', 'user_credentials')
-- ORDER BY ips.avg_fragmentation_in_percent DESC;

-- ============================================================================
-- BACKUP AND RESTORE
-- ============================================================================

-- Backup database:
-- BACKUP DATABASE Studently TO DISK = 'C:\Backups\Studently.bak' WITH INIT;

-- Backup specific tables (requires third-party tools or export/import):
-- Use SQL Server Import/Export Wizard or bcp utility

-- Restore database:
-- RESTORE DATABASE Studently FROM DISK = 'C:\Backups\Studently.bak' WITH REPLACE;

-- ============================================================================
-- DATABASE USER SETUP (Security)
-- ============================================================================

-- Create SQL Server login
-- CREATE LOGIN app_user WITH PASSWORD = 'Secure_Password_Here!123';
-- GO

-- Create database user
-- USE Studently;
-- CREATE USER app_user FOR LOGIN app_user;
-- GO

-- Grant permissions
-- GRANT SELECT, INSERT, UPDATE, DELETE ON users TO app_user;
-- GRANT SELECT, INSERT, UPDATE, DELETE ON user_credentials TO app_user;
-- GO

-- ============================================================================
-- VERIFICATION QUERIES
-- ============================================================================

-- Verify table creation
-- SELECT TABLE_NAME
-- FROM INFORMATION_SCHEMA.TABLES
-- WHERE TABLE_SCHEMA = 'dbo' AND TABLE_TYPE = 'BASE TABLE';

-- Verify indexes
-- SELECT
--     OBJECT_NAME(object_id) AS table_name,
--     name AS index_name,
--     type_desc
-- FROM sys.indexes
-- WHERE OBJECT_NAME(object_id) = 'users';

-- Verify sample data
-- SELECT * FROM users;