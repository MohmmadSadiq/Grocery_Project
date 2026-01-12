CREATE PROCEDURE spCompany_AddNew
    @CompanyName NVARCHAR(150),
    @ContactPersonID INT,
    @Phone NVARCHAR(20),
    @Email NVARCHAR(100),
    @Address NVARCHAR(500),
    @CountryID INT,
    @CommercialNumber NVARCHAR(50),
    @CreatedByUserID INT,
    @NewCompanyID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Companies (
        CompanyName, ContactPersonID, Phone, Email, Address, CountryID, CommercialNumber, CreatedByUserID
        , CreatedDate , IsDeleted
    )
    VALUES (
        @CompanyName, @ContactPersonID, @Phone, @Email, @Address, @CountryID, @CommercialNumber, @CreatedByUserID
        , GETDATE() , 0
    );

    SET @NewCompanyID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE spCompany_GetAll AS
BEGIN
    SET NOCOUNT ON;

    SELECT CompanyID, CompanyName, ContactPersonID, Phone, Email, Address, CountryID, CommercialNumber, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID FROM Companies WHERE IsDeleted = 0;
END
GO

CREATE PROCEDURE spCompany_GetByID
    @CompanyID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CompanyID, CompanyName, ContactPersonID, Phone, Email, Address, CountryID, CommercialNumber, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID FROM Companies WHERE CompanyID = @CompanyID AND IsDeleted = 0;
END
GO

CREATE PROCEDURE spCompany_Update
    @CompanyID INT,
    @CompanyName NVARCHAR(150),
    @ContactPersonID INT,
    @Phone NVARCHAR(20),
    @Email NVARCHAR(100),
    @Address NVARCHAR(500),
    @CountryID INT,
    @CommercialNumber NVARCHAR(50),
    @UpdatedByUserID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Companies SET
        CompanyName = @CompanyName,
        ContactPersonID = @ContactPersonID,
        Phone = @Phone,
        Email = @Email,
        Address = @Address,
        CountryID = @CountryID,
        CommercialNumber = @CommercialNumber,
        UpdatedByUserID = @UpdatedByUserID,
        UpdatedDate = GETDATE()
    WHERE CompanyID = @CompanyID;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

CREATE PROCEDURE spCompany_Delete
    @CompanyID INT,
    @UpdatedByUserID INT
AS 
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsCompleted INT;

    BEGIN TRY
        BEGIN TRANSACTION

        -- Attempt to delete (Intercepted by Trigger)
        DELETE FROM Companies
        WHERE CompanyID = @CompanyID AND IsDeleted != 1

        -- If ID didn't exist or was already deleted
        IF @@ROWCOUNT = 0
            THROW 51000, 'No record found to delete', 1;

        -- Update audit info
        UPDATE Companies
        SET UpdatedByUserID = @UpdatedByUserID,
            UpdatedDate = GETDATE()
        WHERE CompanyID = @CompanyID

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

CREATE TRIGGER CompanySoftDelete
    ON Companies
    INSTEAD OF DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE Companies
    SET IsDeleted = 1
    WHERE CompanyID IN (SELECT CompanyID FROM deleted)
      AND IsDeleted != 1
END
GO

