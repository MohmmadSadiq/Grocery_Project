/* ==========================================================================
   User Management Database Logic
   --------------------------------------------------------------------------
   Description: 
   This script contains Stored Procedures for CRUD operations (Create, Read, 
   Update, Delete) on the [Users] table, and an INSTEAD OF trigger to 
   handle Soft Deletes (marking as deleted instead of removing data).
   ========================================================================== */

-- =============================================
-- Procedure: spAddNewUser
-- Description: Creates a new user record.
-- Returns: The new UserID via the OUTPUT parameter.
-- =============================================
ALTER PROCEDURE spAddNewUser
    @PersonID        INT,
    @UserName        NVARCHAR(50),
    @PasswordHash    NVARCHAR(128),
    @PasswordSalt    NVARCHAR(128),
    @IsActive        BIT,
    @CreatedByUserID INT,
    @NewUserID       INT OUTPUT       -- Holds the generated ID to return to caller
AS
BEGIN
    SET NOCOUNT ON; -- Prevents "X rows affected" messages to improve performance

    INSERT INTO Users
    (
        PersonID,
        UserName,
        PasswordHash,
        PasswordSalt,
        IsActive,
        CreatedByUserID,
        IsDeleted        -- Defaulting to 0 (Not Deleted) on creation
    )
    VALUES
    (
        @PersonID,
        @UserName,
        @PasswordHash,
        @PasswordSalt,
        @IsActive,
        @CreatedByUserID,
        0                -- Hardcoded 0 for IsDeleted
    )

    -- Capture the ID of the newly created row
    SET @NewUserID = SCOPE_IDENTITY();
END
GO

-- =============================================
-- Procedure: spGetUserInfoByID
-- Description: Retrieves a single user by their ID.
-- Note: Only returns users who are NOT deleted.
-- =============================================
ALTER PROCEDURE spGetUserInfoByID
    @UserID INT
AS 
BEGIN
    SET NOCOUNT ON;

    SELECT 
        UserID,
        PersonID,
        UserName,
        PasswordHash,
        PasswordSalt,
        IsActive,
        CreatedDate,
        CreatedByUserID,
        UpdatedDate,
        UpdatedByUserID
    FROM USERS
    WHERE UserID = @UserID 
      AND IsDeleted != 1  -- Filter out soft-deleted records
END
GO

-- =============================================
-- Procedure: spGetAllUsers
-- Description: Retrieves a list of ALL active (non-deleted) users.
-- =============================================
ALTER PROCEDURE spGetAllUsers
AS 
BEGIN
    SET NOCOUNT ON;

    SELECT 
        UserID,
        PersonID,
        UserName,
        PasswordHash,
        PasswordSalt,
        IsActive,
        CreatedDate,
        CreatedByUserID,
        UpdatedDate,
        UpdatedByUserID
    FROM USERS
    WHERE IsDeleted != 1 -- Global filter for active records only
END
GO

-- =============================================
-- Procedure: spUpdateUser
-- Description: Updates user details and tracks who made the change.
-- Returns: 1 if successful, 0 if no row was found.
-- =============================================
ALTER PROCEDURE spUpdateUser
    @UserID          INT,
    @PersonID        INT,
    @UserName        NVARCHAR(50),
    @PasswordHash    NVARCHAR(128),
    @PasswordSalt    NVARCHAR(128),
    @IsActive        BIT,
    @UpdatedByUserID INT 
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Users
    SET             
        PersonID        = @PersonID,
        UserName        = @UserName,
        PasswordHash    = @PasswordHash,
        PasswordSalt    = @PasswordSalt,
        IsActive        = @IsActive,
        UpdatedByUserID = @UpdatedByUserID, -- Audit who updated it
        UpdatedDate     = GETDATE()         -- Audit when it was updated
    WHERE UserID = @UserID

    -- Check if any rows were actually modified
    IF @@ROWCOUNT > 0
        RETURN 1; -- Success
    ELSE
        RETURN 0; -- Failure (ID likely not found)
END
GO

-- =============================================
-- Procedure: spDeleteUser
-- Description: Performs a transactional deletion logic.
-- Note: This issues a DELETE command, which triggers 'UserSoftDelete'.
--       It explicitly updates audit info (UpdatedBy) before finishing.
-- =============================================
ALTER PROCEDURE spDeleteUser
    @UserID INT,
    @UpdatedByUserID INT
AS 
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsCompleted INT;

    BEGIN TRY
        BEGIN TRANSACTION -- Start transaction to ensure atomicity

        -- 1. Attempt to delete.
        -- Because of the 'UserSoftDelete' trigger below, this will NOT physically remove the row.
        -- It will instead update IsDeleted = 1.
        DELETE FROM Users 
        WHERE UserID = @UserID AND IsDeleted != 1

        -- If the ID didn't exist or was already deleted, throw error
        IF @@ROWCOUNT = 0
            THROW 51000, 'No record found to delete', 1;
        
        -- 2. Update audit information specifically for the deletion event
        UPDATE Users
        SET UpdatedByUserID = @UpdatedByUserID,
            UpdatedDate = GETDATE()
        WHERE UserID = @UserID

        SET @IsCompleted = @@ROWCOUNT;

        COMMIT TRANSACTION -- Save changes
    END TRY
    BEGIN CATCH 
        -- If any error occurs, rollback everything
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END
    END CATCH

    -- Return status
    IF @IsCompleted > 0
        RETURN 1;
    ELSE
        RETURN 0;
END
GO

-- =============================================
-- Trigger: UserSoftDelete
-- Description: Intercepts DELETE commands on the [Users] table.
-- Action: Instead of deleting, it updates the row to set IsDeleted = 1.
-- =============================================
ALTER TRIGGER UserSoftDelete 
   ON Users
   INSTEAD OF DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    
    -- Perform the "Soft Delete"
    -- Joins with the 'deleted' virtual table to find which rows were targeted
    UPDATE Users
    SET IsDeleted = 1
    WHERE UserID IN (SELECT UserID FROM deleted) 
      AND IsDeleted != 1
END
GO