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

