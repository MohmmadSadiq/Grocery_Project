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

