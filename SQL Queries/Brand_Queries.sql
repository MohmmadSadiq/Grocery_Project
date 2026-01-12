CREATE PROCEDURE spBrand_AddNew
    @BrandName NVARCHAR(100),
    @CompanyID INT,
    @Description NVARCHAR(MAX),
    @CreatedByUserID INT,
    @NewBrandID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Brands (
        BrandName, CompanyID, Description, CreatedByUserID
        , CreatedDate , IsDeleted
    )
    VALUES (
        @BrandName, @CompanyID, @Description, @CreatedByUserID
        , GETDATE() , 0
    );

    SET @NewBrandID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE spBrand_GetAll AS
BEGIN
    SET NOCOUNT ON;

    SELECT BrandID, BrandName, CompanyID, Description, CreatedDate, CreatedByUserID FROM Brands WHERE IsDeleted = 0;
END
GO

CREATE PROCEDURE spBrand_GetByID
    @BrandID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT BrandID, BrandName, CompanyID, Description, CreatedDate, CreatedByUserID FROM Brands WHERE BrandID = @BrandID AND IsDeleted = 0;
END
GO

CREATE PROCEDURE spBrand_Update
    @BrandID INT,
    @BrandName NVARCHAR(100),
    @CompanyID INT,
    @Description NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Brands SET
        BrandName = @BrandName,
        CompanyID = @CompanyID,
        Description = @Description
    WHERE BrandID = @BrandID;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

ALTER PROCEDURE spBrand_Delete
    @BrandID INT
AS 
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsCompleted INT;

    BEGIN TRY
        BEGIN TRANSACTION

        -- Attempt to delete (Intercepted by Trigger)
        DELETE FROM Brands
        WHERE BrandID = @BrandID AND IsDeleted != 1

		SET @IsCompleted = @@ROWCOUNT;
        -- If ID didn't exist or was already deleted
        IF @@ROWCOUNT = 0
            THROW 51000, 'No record found to delete', 1;


        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
    END CATCH

    IF @IsCompleted > 0 RETURN 1; ELSE RETURN 0;
END
GO

CREATE TRIGGER BrandSoftDelete
    ON Brands
    INSTEAD OF DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE Brands
    SET IsDeleted = 1
    WHERE BrandID IN (SELECT BrandID FROM deleted)
      AND IsDeleted != 1
END
GO

