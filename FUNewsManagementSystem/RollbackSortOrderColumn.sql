-- Rollback Migration: Remove SortOrder column from Category table
-- Date: 2025-11-07
-- Description: Remove SortOrder column if needed

USE FUNewsManagementSystem;
GO

-- Check if column exists before removing
IF EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Category'
    AND COLUMN_NAME = 'SortOrder'
)
BEGIN
    -- Drop SortOrder column
    ALTER TABLE Category
    DROP COLUMN SortOrder;

    PRINT 'SortOrder column removed successfully';
END
ELSE
BEGIN
    PRINT 'SortOrder column does not exist';
END
GO

PRINT 'Rollback completed successfully';
GO
