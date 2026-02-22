
ALTER  PROCEDURE spPeople_GetByID
    @PersonID INT
AS
BEGIN
    SELECT PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName,
           DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath,
           CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID
    FROM People
    WHERE PersonID = @PersonID
      AND ISNULL(IsDeleted, 0) = 0;
END
GO

ALTER  PROCEDURE spPeople_AddNew
    @NationalNo NVARCHAR(20) = NULL,
    @FirstName NVARCHAR(50),
    @SecondName NVARCHAR(50) = NULL,
    @ThirdName NVARCHAR(50) = NULL,
    @LastName NVARCHAR(50),
    @DateOfBirth DATE = NULL,
    @Gender TINYINT = NULL,
    @Address NVARCHAR(500) = NULL,
    @Phone NVARCHAR(20) = NULL,
    @Email NVARCHAR(100) = NULL,
    @NationalityCountryID INT = NULL,
    @ImagePath NVARCHAR(250) = NULL,
    @CreatedDate DATETIME = NULL,
    @CreatedByUserID INT = NULL,
    @UpdatedDate DATETIME = NULL,
    @UpdatedByUserID INT = NULL,
    @NewPersonID INT OUTPUT
AS
BEGIN
    INSERT INTO People (NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID)
    VALUES (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth, @Gender, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath, ISNULL(@CreatedDate, GETDATE()), @CreatedByUserID, @UpdatedDate, @UpdatedByUserID);
    SET @NewPersonID = SCOPE_IDENTITY();
END
GO


ALTER    PROCEDURE spPeople_Update
    @PersonID INT,
    @NationalNo NVARCHAR(20) = NULL,
    @FirstName NVARCHAR(50),
    @SecondName NVARCHAR(50) = NULL,
    @ThirdName NVARCHAR(50) = NULL,
    @LastName NVARCHAR(50),
    @DateOfBirth DATE = NULL,
    @Gender TINYINT = NULL,
    @Address NVARCHAR(500) = NULL,
    @Phone NVARCHAR(20) = NULL,
    @Email NVARCHAR(100) = NULL,
    @NationalityCountryID INT = NULL,
    @ImagePath NVARCHAR(250) = NULL,
    @UpdatedDate DATETIME = NULL,
    @UpdatedByUserID INT = NULL
AS
BEGIN
    -- Block update if the record is soft-deleted
    IF NOT EXISTS (SELECT 1 FROM People WHERE PersonID = @PersonID AND ISNULL(IsDeleted, 0) = 0)
        RETURN 0;

    UPDATE People
    SET NationalNo = @NationalNo,
        FirstName = @FirstName,
        SecondName = @SecondName,
        ThirdName = @ThirdName,
        LastName = @LastName,
        DateOfBirth = @DateOfBirth,
        Gender = @Gender,
        Address = @Address,
        Phone = @Phone,
        Email = @Email,
        NationalityCountryID = @NationalityCountryID,
        ImagePath = @ImagePath,
        UpdatedDate = ISNULL(@UpdatedDate, GETDATE()),
        UpdatedByUserID = @UpdatedByUserID
    WHERE PersonID = @PersonID
      AND ISNULL(IsDeleted, 0) = 0;
    RETURN @@ROWCOUNT;
END
GO


ALTER    PROCEDURE spPeople_Delete
    @PersonID INT,
    @UpdatedByUserID INT
AS 
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsCompleted INT;

    BEGIN TRY
        BEGIN TRANSACTION

        -- Attempt to delete (Intercepted by Trigger)
        DELETE FROM People
        WHERE PersonID = @PersonID AND IsDeleted != 1

        -- If ID didn't exist or was already deleted
        IF @@ROWCOUNT = 0
            THROW 51000, 'No record found to delete', 1;

        -- Update audit info
        UPDATE People
        SET UpdatedByUserID = @UpdatedByUserID,
            UpdatedDate = GETDATE()
        WHERE PersonID = @PersonID

        SET @IsCompleted = @@ROWCOUNT;
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
    END CATCH

    IF @IsCompleted > 0 RETURN 1; ELSE RETURN 0;
END
GO


ALTER    PROCEDURE spPeople_GetAll
AS
BEGIN
    SELECT PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName,
           DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath,
           CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID
    FROM People
    WHERE ISNULL(IsDeleted, 0) = 0;
END
GO



-- Soft Delete Trigger for People
ALTER  TRIGGER PeopleSoftDelete
    ON People
    INSTEAD OF DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE People
    SET IsDeleted = 1
    WHERE PersonID IN (SELECT PersonID FROM deleted)
      AND IsDeleted != 1;
END
GO


-- ============================================================
-- GET PERSON BY NATIONAL NO
-- ============================================================
CREATE OR ALTER PROCEDURE spPeople_GetByNationalNo
    @NationalNo NVARCHAR(20)
AS
BEGIN
    SELECT PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName,
           DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath,
           CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID
    FROM People
    WHERE NationalNo = @NationalNo
      AND ISNULL(IsDeleted, 0) = 0;
END
GO


-- ============================================================
-- GET PERSON BY EMAIL
-- ============================================================
CREATE OR ALTER PROCEDURE spPeople_GetByEmail
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName,
           DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath,
           CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID
    FROM People
    WHERE Email = @Email
      AND ISNULL(IsDeleted, 0) = 0;
END
GO


-- ============================================================
-- GET PERSON BY PHONE
-- ============================================================
CREATE OR ALTER PROCEDURE spPeople_GetByPhone
    @Phone NVARCHAR(20)
AS
BEGIN
    SELECT PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName,
           DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath,
           CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID
    FROM People
    WHERE Phone = @Phone
      AND ISNULL(IsDeleted, 0) = 0;
END
GO

