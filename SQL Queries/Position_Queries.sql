USE [RMS];
GO

CREATE PROCEDURE spPosition_AddNew
    @PositionName NVARCHAR(50),
    @Description NVARCHAR(MAX),
    @CreatedByUserID INT,
    @NewPositionID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Positions (
        PositionName, Description, CreatedByUserID
        , CreatedDate , IsDeleted
    )
    VALUES (
        @PositionName, @Description, @CreatedByUserID
        , GETDATE() , 0
    );

    SET @NewPositionID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE spPosition_GetAll AS
BEGIN
    SET NOCOUNT ON;

    SELECT PositionID, PositionName, Description, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID FROM Positions WHERE IsDeleted = 0;
END
GO

CREATE PROCEDURE spPosition_GetByID
    @PositionID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT PositionID, PositionName, Description, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID FROM Positions WHERE PositionID = @PositionID AND IsDeleted = 0;
END
GO

CREATE PROCEDURE spPosition_Update
    @PositionID INT,
    @PositionName NVARCHAR(50),
    @Description NVARCHAR(MAX),
    @UpdatedByUserID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Positions SET
        PositionName = @PositionName,
        Description = @Description,
        UpdatedByUserID = @UpdatedByUserID
,
        UpdatedDate = GETDATE()
    WHERE PositionID = @PositionID;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

CREATE PROCEDURE spPosition_Delete
    @PositionID INT,
    @UpdatedByUserID INT

AS 
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsCompleted INT;

    BEGIN TRY
        BEGIN TRANSACTION

        -- Attempt to delete (Intercepted by Trigger)
        DELETE FROM Positions
        WHERE PositionID = @PositionID AND IsDeleted != 1

        SET @IsCompleted = @@ROWCOUNT;
        -- If ID didn't exist or was already deleted
        IF @@ROWCOUNT = 0
            THROW 51000, 'No record found to delete', 1;

        -- Update audit info
        UPDATE Positions
        SET UpdatedByUserID = @UpdatedByUserID,
            UpdatedDate = GETDATE()
        WHERE PositionID = @PositionID

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
    END CATCH

    IF @IsCompleted > 0 RETURN 1; ELSE RETURN 0;
END
GO

CREATE TRIGGER PositionSoftDelete
    ON Positions
    INSTEAD OF DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE Positions
    SET IsDeleted = 1
    WHERE PositionID IN (SELECT PositionID FROM deleted)
      AND IsDeleted != 1
END
GO

