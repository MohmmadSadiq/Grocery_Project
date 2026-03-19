USE [RMS];
GO

CREATE PROCEDURE spProductUnit_AddNew
    @ProductID INT,
    @UnitID INT,
    @Description NVARCHAR,
    @ConversionFactor DECIMAL(18,4),
    @SalePrice DECIMAL(18,2),
    @Barcode NVARCHAR(50),
    @IsActive BIT,
    @CreatedByUserID INT,
    @NewProductUnitID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ProductUnits (
        ProductID, UnitID, Description, ConversionFactor, SalePrice, Barcode, IsActive, CreatedByUserID
        , CreatedDate , IsDeleted
    )
    VALUES (
        @ProductID, @UnitID, @Description, @ConversionFactor, @SalePrice, @Barcode, @IsActive, @CreatedByUserID
        , GETDATE() , 0
    );

    SET @NewProductUnitID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE spProductUnit_GetAll AS
BEGIN
    SET NOCOUNT ON;

    SELECT ProductUnitID, ProductID, UnitID, Description, ConversionFactor, SalePrice, Barcode, IsActive, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID FROM ProductUnits WHERE IsDeleted = 0;
END
GO

CREATE PROCEDURE spProductUnit_GetByID
    @ProductUnitID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ProductUnitID, ProductID, UnitID, Description, ConversionFactor, SalePrice, Barcode, IsActive, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID FROM ProductUnits WHERE ProductUnitID = @ProductUnitID AND IsDeleted = 0;
END
GO

CREATE PROCEDURE spProductUnit_Update
    @ProductUnitID INT,
    @ProductID INT,
    @UnitID INT,
    @Description NVARCHAR,
    @ConversionFactor DECIMAL(18,4),
    @SalePrice DECIMAL(18,2),
    @Barcode NVARCHAR(50),
    @IsActive BIT,
    @UpdatedByUserID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ProductUnits SET
        ProductID = @ProductID,
        UnitID = @UnitID,
        Description = @Description,
        ConversionFactor = @ConversionFactor,
        SalePrice = @SalePrice,
        Barcode = @Barcode,
        IsActive = @IsActive,
        UpdatedByUserID = @UpdatedByUserID
,
        UpdatedDate = GETDATE()
    WHERE ProductUnitID = @ProductUnitID;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

CREATE PROCEDURE spProductUnit_Delete
    @ProductUnitID INT,
    @UpdatedByUserID INT

AS 
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsCompleted INT;

    BEGIN TRY
        BEGIN TRANSACTION

        -- Attempt to delete (Intercepted by Trigger)
        DELETE FROM ProductUnits
        WHERE ProductUnitID = @ProductUnitID AND IsDeleted != 1

        SET @IsCompleted = @@ROWCOUNT;
        -- If ID didn't exist or was already deleted
        IF @@ROWCOUNT = 0
            THROW 51000, 'No record found to delete', 1;

        -- Update audit info
        UPDATE ProductUnits
        SET UpdatedByUserID = @UpdatedByUserID,
            UpdatedDate = GETDATE()
        WHERE ProductUnitID = @ProductUnitID

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
    END CATCH

    IF @IsCompleted > 0 RETURN 1; ELSE RETURN 0;
END
GO

CREATE TRIGGER ProductUnitSoftDelete
    ON ProductUnits
    INSTEAD OF DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE ProductUnits
    SET IsDeleted = 1
    WHERE ProductUnitID IN (SELECT ProductUnitID FROM deleted)
      AND IsDeleted != 1
END
GO

ALTER PROCEDURE spProductUnit_GetByBarcode
    @Barcode NVARCHAR (100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ProductUnitID, ProductID, UnitID, Description, ConversionFactor, SalePrice, Barcode, IsActive, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID FROM ProductUnits WHERE Barcode = @Barcode AND IsDeleted = 0;
END

GO 

ALTER PROCEDURE spProductUnit_SearchByBarcode
    @Barcode NVARCHAR(100),
    @PageNumber INT = NULL,
    @PageSize INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @PageNumber IS NULL OR @PageSize IS NULL
    BEGIN
        -- ���� Pagination - ���� �� �������
        SELECT ProductUnitID, ProductID, UnitID, Description, ConversionFactor, 
               SalePrice, Barcode, IsActive, CreatedDate, CreatedByUserID, 
               UpdatedDate, UpdatedByUserID 
        FROM ProductUnits 
        WHERE Barcode LIKE @Barcode + '%' AND IsDeleted = 0
        ORDER BY ProductUnitID;
    END
    ELSE
    BEGIN
        -- �� Pagination
        SELECT ProductUnitID, ProductID, UnitID, Description, ConversionFactor, 
               SalePrice, Barcode, IsActive, CreatedDate, CreatedByUserID, 
               UpdatedDate, UpdatedByUserID 
        FROM ProductUnits 
        WHERE Barcode LIKE @Barcode + '%' AND IsDeleted = 0
        ORDER BY ProductUnitID
        OFFSET (@PageNumber - 1) * @PageSize ROWS
        FETCH NEXT @PageSize ROWS ONLY;
    END
END
go

CREATE OR ALTER PROCEDURE spProductUnit_SearchActiveWithProductPaged
    @SearchText NVARCHAR(200) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;

    SET @SearchText = LTRIM(RTRIM(ISNULL(@SearchText, '')));

    IF @PageNumber < 1 SET @PageNumber = 1;
    IF @PageSize < 1 SET @PageSize = 20;

    SELECT
        PU.ProductUnitID,
        PU.ProductID,
        PU.UnitID,
        PU.Description,
        PU.ConversionFactor,
        PU.SalePrice,
        PU.Barcode,
        PU.IsActive,
        PU.CreatedDate,
        PU.CreatedByUserID,
        PU.UpdatedDate,
        PU.UpdatedByUserID,
        P.ProductID AS Product_ProductID,
        P.ProductName AS Product_ProductName,
        P.CategoryID AS Product_CategoryID,
        P.BrandID AS Product_BrandID,
        P.Description AS Product_Description,
        P.IsActive AS Product_IsActive,
        P.ReorderLevel AS Product_ReorderLevel,
        P.ImagePath AS Product_ImagePath,
        P.CreatedDate AS Product_CreatedDate,
        P.CreatedByUserID AS Product_CreatedByUserID,
        P.UpdatedDate AS Product_UpdatedDate,
        P.UpdatedByUserID AS Product_UpdatedByUserID,
        COUNT(*) OVER() AS TotalCount
    FROM ProductUnits PU
    INNER JOIN Products P
        ON P.ProductID = PU.ProductID
    WHERE
        PU.IsDeleted = 0
        AND P.IsDeleted = 0
        AND PU.IsActive = 1
        AND P.IsActive = 1
        AND (
            @SearchText = ''
            OR PU.Barcode LIKE @SearchText + '%'
            OR P.ProductName LIKE @SearchText + '%'
        )
    ORDER BY P.ProductName, PU.ProductUnitID
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

ALTER PROCEDURE spProductUnit_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        PU.ProductUnitID,
        PU.ProductID,
        P.ProductName,
        PU.UnitID,
        PU.Description,
        PU.ConversionFactor,
        PU.SalePrice,
        PU.Barcode,
        PU.IsActive AS ProductUnitIsActive,
        P.IsActive AS ProductIsActive,
        PU.CreatedDate,
        PU.CreatedByUserID,
        PU.UpdatedDate,
        PU.UpdatedByUserID
    FROM ProductUnits PU
    INNER JOIN Products P
        ON P.ProductID = PU.ProductID
    WHERE
        PU.IsDeleted = 0
        AND P.IsDeleted = 0
        AND PU.IsActive = 1
        AND P.IsActive = 1
    ORDER BY PU.ProductUnitID;
END
GO