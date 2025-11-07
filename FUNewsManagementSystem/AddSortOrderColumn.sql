-- Migration: Add SortOrder column to Category table
-- Date: 2025-11-07
-- Description: Add SortOrder column to support drag-and-drop ordering

USE FUNewsManagementSystem;
GO

-- Check if column exists before adding
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Category'
    AND COLUMN_NAME = 'SortOrder'
)
BEGIN
    -- Add SortOrder column with default value 0
    ALTER TABLE Category
    ADD SortOrder INT NOT NULL DEFAULT 0;

    PRINT 'SortOrder column added successfully';
END
ELSE
BEGIN
    PRINT 'SortOrder column already exists';
END
GO

-- Update existing categories with sequential sort order
UPDATE Category
SET SortOrder = CategoryID
WHERE SortOrder = 0;
GO

PRINT 'Migration completed successfully';
GO
