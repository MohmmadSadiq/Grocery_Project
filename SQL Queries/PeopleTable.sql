/* -------------------------------------------------------------------------
   1. CREATE (INSERT)
   Returns the new PersonID immediately after insertion.
------------------------------------------------------------------------- */
CREATE PROCEDURE spPeople_AddNew
    @NationalNo           NVARCHAR(20),
    @FirstName            NVARCHAR(50),
    @SecondName           NVARCHAR(50),
    @ThirdName            NVARCHAR(50),
    @LastName             NVARCHAR(50),
    @DateOfBirth          DATE,
    @Gender               TINYINT,
    @Address              NVARCHAR(500),
    @Phone                NVARCHAR(20),
    @Email                NVARCHAR(100),
    @NationalityCountryID INT,
    @ImagePath            NVARCHAR(250),
    @CreatedByUserID      INT,
    @NewPersonID          INT OUTPUT -- Output parameter to return the new ID
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO People (
        NationalNo, FirstName, SecondName, ThirdName, LastName, 
        DateOfBirth, Gender, Address, Phone, Email, 
        NationalityCountryID, ImagePath, CreatedByUserID, 
        CreatedDate, IsDeleted
    )
    VALUES (
        @NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, 
        @DateOfBirth, @Gender, @Address, @Phone, @Email, 
        @NationalityCountryID, @ImagePath, @CreatedByUserID, 
        GETDATE(), 0
    );

    SET @NewPersonID = SCOPE_IDENTITY();
END
GO

/* -------------------------------------------------------------------------
   2. READ (SELECT ALL)
   Only selects active records (IsDeleted = 0).
------------------------------------------------------------------------- */
CREATE PROCEDURE spPeople_GetAll 
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, FullName,
        DateOfBirth, Gender, Address, Phone, Email, 
        NationalityCountryID, ImagePath, CreatedDate, CreatedByUserID, 
        UpdatedDate, UpdatedByUserID
    FROM People
    WHERE IsDeleted = 0;
END
GO

/* -------------------------------------------------------------------------
   3. READ (SELECT BY ID)
   Fetches a single person by Primary Key.
------------------------------------------------------------------------- */
CREATE PROCEDURE spPeople_GetByID
    @PersonID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, FullName,
        DateOfBirth, Gender, Address, Phone, Email, 
        NationalityCountryID, ImagePath, CreatedDate, CreatedByUserID, 
        UpdatedDate, UpdatedByUserID
    FROM People
    WHERE PersonID = @PersonID AND IsDeleted = 0;
END
GO

/* -------------------------------------------------------------------------
   4. UPDATE
   Updates person details and sets the UpdatedDate automatically.
------------------------------------------------------------------------- */
CREATE PROCEDURE spPeople_Update
    @PersonID             INT,
    @NationalNo           NVARCHAR(20),
    @FirstName            NVARCHAR(50),
    @SecondName           NVARCHAR(50),
    @ThirdName            NVARCHAR(50),
    @LastName             NVARCHAR(50),
    @DateOfBirth          DATE,
    @Gender               TINYINT,
    @Address              NVARCHAR(500),
    @Phone                NVARCHAR(20),
    @Email                NVARCHAR(100),
    @NationalityCountryID INT,
    @ImagePath            NVARCHAR(250),
    @UpdatedByUserID      INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE People
    SET 
        NationalNo           = @NationalNo,
        FirstName            = @FirstName,
        SecondName           = @SecondName,
        ThirdName            = @ThirdName,
        LastName             = @LastName,
        DateOfBirth          = @DateOfBirth,
        Gender               = @Gender,
        Address              = @Address,
        Phone                = @Phone,
        Email                = @Email,
        NationalityCountryID = @NationalityCountryID,
        ImagePath            = @ImagePath,
        UpdatedByUserID      = @UpdatedByUserID,
        UpdatedDate          = GETDATE()
    WHERE PersonID = @PersonID;

	IF @@ROWCOUNT > 0
		RETURN 1
	ELSE
		RETURN 0
END
GO

/* -------------------------------------------------------------------------
   5. DELETE (SOFT DELETE)
   Marks the record as deleted and logs who did it.
------------------------------------------------------------------------- */
CREATE PROCEDURE spPeople_Delete
    @PersonID        INT,
    @UpdatedByUserID INT -- Optional: Track who deleted the user
AS
BEGIN
    SET NOCOUNT ON;
	
	 
	BEGIN TRY
	BEGIN TRANSACTION
	
	DELETE FROM People 
	WHERE PersonID = @PersonID AND IsDeleted != 1

	
	IF @@ROWCOUNT = 0
		THROW 51000, 'No record found to delete', 1;
		

	UPDATE People
	SET UpdatedByUserID = @UpdatedByUserID,
		UpdatedDate = GETDATE()
	WHERE PersonID = @PersonID
	DECLARE @IsCompleted int = @@ROWCOUNT;
	COMMIT TRANSACTION

	END TRY
	BEGIN CATCH 

	IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

	END CATCH

	IF @IsCompleted > 0
		RETURN 1
	ELSE
		RETURN 0

END
GO

/* -------------------------------------------------------------------------
   Triggers 
------------------------------------------------------------------------- */

-- INSTED OF DELETE Trigger For Soft Deleting
ALTER TRIGGER PersonSoftDelete 
   ON  People
   INSTEAD OF DELETE
AS 
BEGIN
	SET NOCOUNT ON;
	
	UPDATE People
	SET IsDeleted = 1
	WHERE PersonID in  (SELECT PersonID from deleted ) AND IsDeleted != 1
END
GO
 