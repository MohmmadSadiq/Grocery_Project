USE [RMS];
GO

CREATE PROCEDURE spEmployee_AddNew
    @PersonID INT,
    @PositionID INT,
    @HireDate DATE,
    @FireDate DATE,
    @CreatedByUserID INT,
    @NewEmployeeID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Employees (
        PersonID, PositionID, HireDate, FireDate, CreatedByUserID
        , CreatedDate , IsDeleted
    )
    VALUES (
        @PersonID, @PositionID, @HireDate, @FireDate, @CreatedByUserID
        , GETDATE() , 0
    );

    SET @NewEmployeeID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE spEmployee_GetAll AS
BEGIN
    SET NOCOUNT ON;

    SELECT EmployeeID, PersonID, PositionID, HireDate, FireDate, CreatedByUserID, CreatedDate, UpdatedByUserID, UpdatedDate FROM Employees WHERE IsDeleted = 0;
END
GO

CREATE PROCEDURE spEmployee_GetByID
    @EmployeeID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT EmployeeID, PersonID, PositionID, HireDate, FireDate, CreatedByUserID, CreatedDate, UpdatedByUserID, UpdatedDate FROM Employees WHERE EmployeeID = @EmployeeID AND IsDeleted = 0;
END
GO

CREATE PROCEDURE spEmployee_Update
    @EmployeeID INT,
    @PersonID INT,
    @PositionID INT,
    @HireDate DATE,
    @FireDate DATE,
    @UpdatedByUserID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Employees SET
        PersonID = @PersonID,
        PositionID = @PositionID,
        HireDate = @HireDate,
        FireDate = @FireDate,
        UpdatedByUserID = @UpdatedByUserID
,
        UpdatedDate = GETDATE()
    WHERE EmployeeID = @EmployeeID;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

CREATE PROCEDURE spEmployee_Delete
    @EmployeeID INT,
    @UpdatedByUserID INT

AS 
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsCompleted INT;

    BEGIN TRY
        BEGIN TRANSACTION

        -- Attempt to delete (Intercepted by Trigger)
        DELETE FROM Employees
        WHERE EmployeeID = @EmployeeID AND IsDeleted != 1

        SET @IsCompleted = @@ROWCOUNT;
        -- If ID didn't exist or was already deleted
        IF @@ROWCOUNT = 0
            THROW 51000, 'No record found to delete', 1;

        -- Update audit info
        UPDATE Employees
        SET UpdatedByUserID = @UpdatedByUserID,
            UpdatedDate = GETDATE()
        WHERE EmployeeID = @EmployeeID

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
    END CATCH

    IF @IsCompleted > 0 RETURN 1; ELSE RETURN 0;
END
GO

CREATE TRIGGER EmployeeSoftDelete
    ON Employees
    INSTEAD OF DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE Employees
    SET IsDeleted = 1
    WHERE EmployeeID IN (SELECT EmployeeID FROM deleted)
      AND IsDeleted != 1
END
GO

