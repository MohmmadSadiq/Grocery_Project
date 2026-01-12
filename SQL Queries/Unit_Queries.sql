USE [RMS];
GO

CREATE PROCEDURE spUnit_AddNew
    @UnitName NVARCHAR(50),
    @Description NVARCHAR,
    @IsActive BIT,
    @CreatedByUserID INT,
    @NewUnitID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Units (
        UnitName, Description, IsActive, CreatedByUserID
        , CreatedDate , IsDeleted
    )
    VALUES (
        @UnitName, @Description, @IsActive, @CreatedByUserID
        , GETDATE() , 0
    );

    SET @NewUnitID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE spUnit_GetAll AS
BEGIN
    SET NOCOUNT ON;

    SELECT UnitID, UnitName, Description, IsActive, CreatedDate, CreatedByUserID FROM Units WHERE IsDeleted = 0;
END
GO

CREATE PROCEDURE spUnit_GetByID
    @UnitID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT UnitID, UnitName, Description, IsActive, CreatedDate, CreatedByUserID FROM Units WHERE UnitID = @UnitID AND IsDeleted = 0;
END
GO

CREATE PROCEDURE spUnit_Update
    @UnitID INT,
    @UnitName NVARCHAR(50),
    @Description NVARCHAR,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Units SET
        UnitName = @UnitName,
        Description = @Description,
        IsActive = @IsActive
    WHERE UnitID = @UnitID;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

CREATE PROCEDURE spUnit_Delete
    @UnitID INT
AS 
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsCompleted INT;

    BEGIN TRY
        BEGIN TRANSACTION

        -- Attempt to delete (Intercepted by Trigger)
        DELETE FROM Units
        WHERE UnitID = @UnitID AND IsDeleted != 1

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

CREATE TRIGGER UnitSoftDelete
    ON Units
    INSTEAD OF DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE Units
    SET IsDeleted = 1
    WHERE UnitID IN (SELECT UnitID FROM deleted)
      AND IsDeleted != 1
END
GO

