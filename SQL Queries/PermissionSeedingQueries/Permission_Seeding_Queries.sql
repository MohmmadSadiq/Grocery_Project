/*
    File: Permission_Seeding_Queries.sql
    Purpose: Seed RBAC permissions and map them to Admin role.
    Notes:
    - Idempotent: safe to run multiple times.
    - Set @SeedByUserID to a valid UserID if you want audit attribution.
*/

use rms

go


BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @SeedByUserID INT = NULL;
    DECLARE @AdminRoleName NVARCHAR(50) = N'Admin';

    DECLARE @PermissionSeed TABLE
    (
        PermissionName NVARCHAR(100) NOT NULL PRIMARY KEY,
        [Description]  NVARCHAR(MAX) NULL
    );

    INSERT INTO @PermissionSeed (PermissionName, [Description])
    VALUES
        (N'Products.View',                 N'View products list and details'),
        (N'Products.Create',               N'Create new products'),
        (N'Products.Edit',                 N'Edit product data'),
        (N'Products.Delete',               N'Delete products'),
        (N'Products.Activate',             N'Activate products'),
        (N'Products.Deactivate',           N'Deactivate products'),
        (N'Products.Image.Edit',           N'Upload and remove product images'),
        (N'Products.Export',               N'Export products to Excel'),
        (N'Products.Category.Move',        N'Move products between categories'),

        (N'ProductUnits.View',             N'View product units'),
        (N'ProductUnits.Create',           N'Add product units'),
        (N'ProductUnits.Edit',             N'Edit product units'),
        (N'ProductUnits.Delete',           N'Delete product units'),
        (N'ProductUnits.Price.Edit',       N'Edit product unit sale price'),
        (N'ProductUnits.Conversion.Edit',  N'Edit product unit conversion factor'),
        (N'ProductUnits.Barcode.Edit',     N'Edit product unit barcode'),
        (N'ProductUnits.Activate',         N'Activate product units'),
        (N'ProductUnits.Deactivate',       N'Deactivate product units'),

        (N'Units.View',                    N'View global units'),
        (N'Units.Create',                  N'Create global units'),
        (N'Units.Edit',                    N'Edit global units'),
        (N'Units.Delete',                  N'Delete global units'),

        (N'Categories.View',               N'View categories'),
        (N'Categories.Create',             N'Create categories'),
        (N'Categories.Edit',               N'Edit categories'),
        (N'Categories.Delete',             N'Delete categories'),

        (N'Companies.View',                N'List companies and view company details'),
        (N'Companies.Search',              N'Search companies by ID, name, commercial number, phone, or email'),
        (N'Companies.Create',              N'Create new company records'),
        (N'Companies.Edit',                N'Edit company data and contact fields'),
        (N'Companies.Delete',              N'Delete company records'),
        (N'Companies.ContactPerson.Assign',N'Assign or change linked company contact person'),
        (N'Companies.Country.Assign',      N'Assign or change linked company country'),
        (N'Companies.Select',              N'Select companies in finder/config controls'),

        (N'Brands.View',                   N'List and view brands'),
        (N'Brands.Create',                 N'Create new brands'),
        (N'Brands.Edit',                   N'Edit brand data'),
        (N'Brands.Delete',                 N'Delete brands'),
        (N'Brands.Company.Assign',         N'Assign or change parent company for a brand');

    -- 1) Update existing permissions and undelete if needed.
    UPDATE p
    SET
        p.[Description]   = s.[Description],
        p.IsDeleted       = 0,
        p.UpdatedDate     = GETDATE(),
        p.UpdatedByUserID = @SeedByUserID
    FROM Permissions p
    INNER JOIN @PermissionSeed s
        ON s.PermissionName = p.PermissionName;

    -- 2) Insert missing permissions.
    INSERT INTO Permissions
    (
        PermissionName,
        [Description],
        CreatedDate,
        CreatedByUserID,
        UpdatedDate,
        UpdatedByUserID,
        IsDeleted
    )
    SELECT
        s.PermissionName,
        s.[Description],
        GETDATE(),
        @SeedByUserID,
        GETDATE(),
        @SeedByUserID,
        0
    FROM @PermissionSeed s
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM Permissions p
        WHERE p.PermissionName = s.PermissionName
    );

    -- 3) Assign seeded permissions to Admin role.
    DECLARE @AdminRoleID INT;

    SELECT TOP (1) @AdminRoleID = r.RoleID
    FROM Roles r
    WHERE r.RoleName = @AdminRoleName
      AND (r.IsDeleted = 0 OR r.IsDeleted IS NULL);

    IF @AdminRoleID IS NOT NULL
    BEGIN
        INSERT INTO RolePermissions (RoleID, PermissionID, CreatedDate, CreatedByUserID)
        SELECT
            @AdminRoleID,
            p.PermissionID,
            GETDATE(),
            @SeedByUserID
        FROM Permissions p
        INNER JOIN @PermissionSeed s
            ON s.PermissionName = p.PermissionName
        WHERE (p.IsDeleted = 0 OR p.IsDeleted IS NULL)
          AND NOT EXISTS
          (
              SELECT 1
              FROM RolePermissions rp
              WHERE rp.RoleID = @AdminRoleID
                AND rp.PermissionID = p.PermissionID
          );
    END
    ELSE
    BEGIN
        PRINT 'Admin role not found. Permissions were added, but role mapping was skipped.';
    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
