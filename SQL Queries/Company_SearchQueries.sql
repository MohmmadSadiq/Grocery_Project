-- Search Company by CompanyName
CREATE PROCEDURE spCompany_GetByCompanyName
    @CompanyName NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CompanyID, CompanyName, ContactPersonID, Phone, Email, Address, CountryID, CommercialNumber, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID 
    FROM Companies 
    WHERE CompanyName = @CompanyName AND IsDeleted = 0;
END
GO

-- Search Company by CommercialNumber
CREATE PROCEDURE spCompany_GetByCommercialNumber
    @CommercialNumber NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CompanyID, CompanyName, ContactPersonID, Phone, Email, Address, CountryID, CommercialNumber, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID 
    FROM Companies 
    WHERE CommercialNumber = @CommercialNumber AND IsDeleted = 0;
END
GO

-- Search Company by Phone
CREATE PROCEDURE spCompany_GetByPhone
    @Phone NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CompanyID, CompanyName, ContactPersonID, Phone, Email, Address, CountryID, CommercialNumber, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID 
    FROM Companies 
    WHERE Phone = @Phone AND IsDeleted = 0;
END
GO

-- Search Company by Email
CREATE PROCEDURE spCompany_GetByEmail
    @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CompanyID, CompanyName, ContactPersonID, Phone, Email, Address, CountryID, CommercialNumber, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID 
    FROM Companies 
    WHERE Email = @Email AND IsDeleted = 0;
END
GO
