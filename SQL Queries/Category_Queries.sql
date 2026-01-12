CREATE PROCEDURE spCategory_AddNew
    @CategoryName NVARCHAR(100),
    @Description NVARCHAR(MAX),
    @CreatedByUserID INT,
    @NewCategoryID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Categories (
        CategoryName, Description, CreatedByUserID
        , CreatedDate , IsDeleted
    )
    VALUES (
        @CategoryName, @Description, @CreatedByUserID
        , GETDATE() , 0
    );

    SET @NewCategoryID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE spCategory_GetAll AS
BEGIN
    SET NOCOUNT ON;

    SELECT CategoryID, CategoryName, Description, CreatedDate, CreatedByUserID FROM Categories WHERE IsDeleted = 0;
END
GO

CREATE PROCEDURE spCategory_GetByID
    @CategoryID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CategoryID, CategoryName, Description, CreatedDate, CreatedByUserID FROM Categories WHERE CategoryID = @CategoryID AND IsDeleted = 0;
END
GO

CREATE PROCEDURE spCategory_Update
    @CategoryID INT,
    @CategoryName NVARCHAR(100),
    @Description NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Categories SET
        CategoryName = @CategoryName,
        Description = @Description
    WHERE CategoryID = @CategoryID;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

CREATE PROCEDURE spCategory_Delete
    @CategoryID INT
AS 
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsCompleted INT;

    BEGIN TRY
        BEGIN TRANSACTION

        -- Attempt to delete (Intercepted by Trigger)
        DELETE FROM Categories
        WHERE CategoryID = @CategoryID AND IsDeleted != 1

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

CREATE TRIGGER CategorySoftDelete
    ON Categories
    INSTEAD OF DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE Categories
    SET IsDeleted = 1
    WHERE CategoryID IN (SELECT CategoryID FROM deleted)
      AND IsDeleted != 1
END
GO

