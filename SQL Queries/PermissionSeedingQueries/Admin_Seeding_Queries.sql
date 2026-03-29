/*
    File: Admin_Seeding_Queries.sql
    Purpose: Seed Admin role and assign an existing user to Admin role.
    Notes:
    - Idempotent: safe to run multiple times.
    - Set @AdminUserName (or @AdminUserID) to the user you want to promote.
*/
USE rms;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @SeedByUserID INT = NULL;
    DECLARE @AdminRoleName NVARCHAR(50) = N'Admin';

    -- Set one of these to target the user you want to assign to Admin role.
    DECLARE @AdminUserName NVARCHAR(50) = N'admin';
    DECLARE @AdminUserID INT = 3;

    DECLARE @ResolvedAdminRoleID INT;
    DECLARE @ResolvedAdminUserID INT;

    -- 1) Ensure Admin role exists and is active.
    SELECT TOP (1) @ResolvedAdminRoleID = RoleID
    FROM Roles
    WHERE RoleName = @AdminRoleName;

    IF @ResolvedAdminRoleID IS NULL
    BEGIN
        INSERT INTO Roles
        (
            RoleName,
            Description,
            CreatedDate,
            CreatedByUserID,
            UpdatedDate,
            UpdatedByUserID,
            IsDeleted
        )
        VALUES
        (
            @AdminRoleName,
            N'System administrator with full access to all modules and actions.',
            GETDATE(),
            @SeedByUserID,
            GETDATE(),
            @SeedByUserID,
            0
        );

        SET @ResolvedAdminRoleID = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        UPDATE Roles
        SET
            IsDeleted = 0,
            UpdatedDate = GETDATE(),
            UpdatedByUserID = @SeedByUserID
        WHERE RoleID = @ResolvedAdminRoleID;
    END;

    -- 2) Resolve target user by @AdminUserID first, then @AdminUserName.
    IF @AdminUserID IS NOT NULL
    BEGIN
        SELECT TOP (1) @ResolvedAdminUserID = UserID
        FROM Users
        WHERE UserID = @AdminUserID
          AND (IsDeleted = 0 OR IsDeleted IS NULL);
    END;

    IF @ResolvedAdminUserID IS NULL AND @AdminUserName IS NOT NULL
    BEGIN
        SELECT TOP (1) @ResolvedAdminUserID = UserID
        FROM Users
        WHERE UserName = @AdminUserName
          AND (IsDeleted = 0 OR IsDeleted IS NULL);
    END;

    -- 3) Assign Admin role to resolved user if found.
    IF @ResolvedAdminUserID IS NOT NULL
    BEGIN
        INSERT INTO UserRoles (UserID, RoleID, CreatedDate, CreatedByUserID)
        SELECT
            @ResolvedAdminUserID,
            @ResolvedAdminRoleID,
            GETDATE(),
            @SeedByUserID
        WHERE NOT EXISTS
        (
            SELECT 1
            FROM UserRoles ur
            WHERE ur.UserID = @ResolvedAdminUserID
              AND ur.RoleID = @ResolvedAdminRoleID
        );

        PRINT 'Admin role seeded and user mapped successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Admin role seeded, but no user was mapped. Set @AdminUserName or @AdminUserID to map a user.';
    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO
