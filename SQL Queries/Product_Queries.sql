CREATE PROCEDURE spProduct_AddNew
    @ProductName NVARCHAR(200),
    @CategoryID INT,
    @BrandID INT,
    @Description NVARCHAR(MAX),
    @IsActive BIT,
    @ReorderLevel INT,
    @CreatedByUserID INT,
    @NewProductID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Products (
        ProductName, CategoryID, BrandID, Description, IsActive, ReorderLevel, CreatedByUserID
        , CreatedDate , IsDeleted
    )
    VALUES (
        @ProductName, @CategoryID, @BrandID, @Description, @IsActive, @ReorderLevel, @CreatedByUserID
        , GETDATE() , 0
    );

    SET @NewProductID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE spProduct_GetAll AS
BEGIN
    SET NOCOUNT ON;

    SELECT ProductID, ProductName, CategoryID, BrandID, Description, IsActive, ReorderLevel, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID FROM Products WHERE IsDeleted = 0;
END
GO

CREATE PROCEDURE spProduct_GetByID
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ProductID, ProductName, CategoryID, BrandID, Description, IsActive, ReorderLevel, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID FROM Products WHERE ProductID = @ProductID AND IsDeleted = 0;
END
GO

CREATE PROCEDURE spProduct_Update
    @ProductID INT,
    @ProductName NVARCHAR(200),
    @CategoryID INT,
    @BrandID INT,
    @Description NVARCHAR(MAX),
    @IsActive BIT,
    @ReorderLevel INT,
    @UpdatedByUserID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Products SET
        ProductName = @ProductName,
        CategoryID = @CategoryID,
        BrandID = @BrandID,
        Description = @Description,
        IsActive = @IsActive,
        ReorderLevel = @ReorderLevel,
        UpdatedByUserID = @UpdatedByUserID
,
        UpdatedDate = GETDATE()
    WHERE ProductID = @ProductID;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

CREATE PROCEDURE spProduct_Delete
    @ProductID INT,
    @UpdatedByUserID INT

AS 
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsCompleted INT;

    BEGIN TRY
        BEGIN TRANSACTION

        -- Attempt to delete (Intercepted by Trigger)
        DELETE FROM Products
        WHERE ProductID = @ProductID AND IsDeleted != 1

        SET @IsCompleted = @@ROWCOUNT;
        -- If ID didn't exist or was already deleted
        IF @@ROWCOUNT = 0
            THROW 51000, 'No record found to delete', 1;

        -- Update audit info
        UPDATE Products
        SET UpdatedByUserID = @UpdatedByUserID,
            UpdatedDate = GETDATE()
        WHERE ProductID = @ProductID

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
    END CATCH

    IF @IsCompleted > 0 RETURN 1; ELSE RETURN 0;
END
GO

CREATE TRIGGER ProductSoftDelete
    ON Products
    INSTEAD OF DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE Products
    SET IsDeleted = 1
    WHERE ProductID IN (SELECT ProductID FROM deleted)
      AND IsDeleted != 1
END
GO


-- 1. Add the ImagePath column to the Products table
-- We check if the column exists first to avoid errors if you run the script twice
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Products' AND COLUMN_NAME = 'ImagePath')
BEGIN
    ALTER TABLE Products
    ADD ImagePath NVARCHAR(MAX);
END
GO

-- 2. Update the View to include the new ImagePath column
-- We use OR ALTER (available in newer SQL versions) or DROP/CREATE logic
IF OBJECT_ID('ProductView', 'V') IS NOT NULL
    DROP VIEW ProductView;
GO

/*
ProductID
ProductName

Description
IsActive]
ImagePath]
*/

ALTER VIEW ProductView
AS
SELECT 
    Products.ProductID, 
    Products.ProductName, 
    Products.Description, 
    Products.IsActive, 
    Products.ReorderLevel, 
    Products.ImagePath, -- Added ImagePath here
	Products.CategoryID,
	Products.BrandID,
    Categories.CategoryName, 
    Brands.BrandName, 
    Companies.CompanyName
FROM Brands 
    INNER JOIN Companies ON Brands.CompanyID = Companies.CompanyID 
    INNER JOIN Products ON Brands.BrandID = Products.BrandID 
    INNER JOIN Categories ON Products.CategoryID = Categories.CategoryID;
GO

-- 3. Update the Stored Procedure to select the ImagePath
IF OBJECT_ID('GetProductsPaged', 'P') IS NOT NULL
    DROP PROCEDURE GetProductsPaged;
GO

CREATE PROCEDURE GetProductsPaged
(
    @PageNumber INT,
    @RowsPerPage INT
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Validation for PageNumber
    IF @PageNumber < 1
        SET @PageNumber = 1;

    -- Select including ImagePath
    SELECT 
        ProductID, 
        ProductName, 
        Description, 
        IsActive, 
        ReorderLevel, 
        ImagePath, -- Added ImagePath to the result set
        CategoryName, 
        BrandName, 
        CompanyName
    FROM ProductView
    ORDER BY ProductID
    OFFSET (@PageNumber - 1) * @RowsPerPage ROWS
    FETCH NEXT @RowsPerPage ROWS ONLY;
END
GO

ALTER PROCEDURE sp_SearchProductsPages
    @SearchText NVARCHAR(100) = NULL,
    @CategoryId INT = NULL,
    @IsActive   BIT = NULL,
    @PageNumber INT = 1,
    @PageSize   INT = 20,
    @SortBy     NVARCHAR(50) = 'Name'
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    -- ÇáÞÑÇÁÉ ãÈÇÔÑÉ ãä ÇáÜ View
    -- ÅÐÇ ÇÓÊÎÏãÊ ÇáÍá ÇáÃæá (Indexed View)¡ ÃÖÝ (NOEXPAND) ááÊÃßÏ ãä ÇÓÊÎÏÇã ÇáÝåÑÓ Ýí ÈÚÖ äÓÎ SQL
    SELECT 
        ProductID,
        ProductName,
        CategoryName,
        BrandName,
        ReorderLevel,
        IsActive,
        ImagePath,
        COUNT(*) OVER() AS TotalCount
    FROM ProductView 
    WHERE 
        (@IsActive IS NULL OR IsActive = @IsActive)
        AND (@CategoryId IS NULL OR CategoryID = @CategoryId)
        AND (@SearchText IS NULL OR (
            ProductName = @SearchText 
        ))
    ORDER BY 
		CASE WHEN @SortBy = 'ID' THEN ProductID END ASC,
        CASE WHEN @SortBy = 'Name' THEN ProductName END ASC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END