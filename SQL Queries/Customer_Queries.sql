USE [RMS];
GO

IF COL_LENGTH('dbo.Customers', 'IsDeleted') IS NULL
BEGIN
    ALTER TABLE dbo.Customers
    ADD IsDeleted BIT NOT NULL CONSTRAINT DF_Customers_IsDeleted DEFAULT (0);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.check_constraints
    WHERE name = 'CK_Customers_PersonOrCompany'
      AND parent_object_id = OBJECT_ID('dbo.Customers')
)
BEGIN
    ALTER TABLE dbo.Customers
    ADD CONSTRAINT CK_Customers_PersonOrCompany
    CHECK (
        (PersonID IS NOT NULL AND CompanyID IS NULL)
        OR (PersonID IS NULL AND CompanyID IS NOT NULL)
    );
END
GO

CREATE OR ALTER PROCEDURE spCustomer_AddNew
    @PersonID INT = NULL,
    @CompanyID INT = NULL,
    @AccountID INT = NULL,
    @IsActive BIT,
    @CreatedByUserID INT = NULL,
    @NewCustomerID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Customers (
        PersonID, CompanyID, AccountID, IsActive, CreatedByUserID, CreatedDate, IsDeleted
    )
    VALUES (
        @PersonID, @CompanyID, @AccountID, @IsActive, @CreatedByUserID, GETDATE(), 0
    );

    SET @NewCustomerID = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE spCustomer_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CustomerID,
        PersonID,
        CompanyID,
        AccountID,
        IsActive,
        CreatedDate,
        CreatedByUserID,
        UpdatedDate,
        UpdatedByUserID
    FROM Customers
    WHERE IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE spCustomer_GetByID
    @CustomerID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CustomerID,
        PersonID,
        CompanyID,
        AccountID,
        IsActive,
        CreatedDate,
        CreatedByUserID,
        UpdatedDate,
        UpdatedByUserID
    FROM Customers
    WHERE CustomerID = @CustomerID
      AND IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE spCustomer_Update
    @CustomerID INT,
    @PersonID INT = NULL,
    @CompanyID INT = NULL,
    @AccountID INT = NULL,
    @IsActive BIT,
    @UpdatedByUserID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Customers
    SET PersonID = @PersonID,
        CompanyID = @CompanyID,
        AccountID = @AccountID,
        IsActive = @IsActive,
        UpdatedByUserID = @UpdatedByUserID,
        UpdatedDate = GETDATE()
    WHERE CustomerID = @CustomerID
      AND IsDeleted = 0;

    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0;
END
GO

CREATE OR ALTER PROCEDURE spCustomer_Delete
    @CustomerID INT,
    @UpdatedByUserID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IsCompleted INT = 0;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM Customers
        WHERE CustomerID = @CustomerID
          AND IsDeleted <> 1;

        SET @IsCompleted = @@ROWCOUNT;

        IF @IsCompleted = 0
            THROW 51000, 'No record found to delete', 1;

        UPDATE Customers
        SET UpdatedByUserID = @UpdatedByUserID,
            UpdatedDate = GETDATE()
        WHERE CustomerID = @CustomerID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
    END CATCH

    IF @IsCompleted > 0 RETURN 1 ELSE RETURN 0;
END
GO

CREATE OR ALTER TRIGGER CustomerSoftDelete
    ON Customers
    INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Customers
    SET IsDeleted = 1
    WHERE CustomerID IN (SELECT CustomerID FROM deleted)
      AND IsDeleted <> 1;
END
GO

CREATE OR ALTER VIEW Customers_View
AS
SELECT
    C.CustomerID,
    CustomerType = 'Company',
    CO.CompanyName AS CustomerName,
    CO.Phone AS Phone,
    CO.Email AS Email,
    CO.Address AS Address,
    (
        SELECT CountryName
        FROM Countries
        WHERE CountryID = CO.CountryID
    ) AS Country,
    C.IsActive,
    C.CompanyID,
    C.PersonID
FROM Customers C
JOIN Companies CO ON C.CompanyID = CO.CompanyID
WHERE C.IsDeleted = 0

UNION

SELECT
    C.CustomerID,
    CustomerType = 'Person',
    P.FullName AS CustomerName,
    P.Phone AS Phone,
    P.Email AS Email,
    P.Address AS Address,
    (
        SELECT CountryName
        FROM Countries
        WHERE CountryID = P.NationalityCountryID
    ) AS Country,
    C.IsActive,
    C.CompanyID,
    C.PersonID
FROM Customers C
JOIN People P ON C.PersonID = P.PersonID
WHERE C.IsDeleted = 0;
GO

CREATE OR ALTER PROCEDURE sp_SearchCustomerPages
    @SearchText NVARCHAR(100) = NULL,
    @SearchBy NVARCHAR(50) = 'CustomerName',
    @CustomerType NVARCHAR(20) = NULL,
    @IsActive BIT = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 20,
    @SortBy NVARCHAR(50) = 'CustomerName'
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    IF @SortBy NOT IN ('CustomerID', 'CustomerName', 'Phone', 'Country')
        SET @SortBy = 'CustomerName';

    IF @PageNumber < 1
        SET @PageNumber = 1;

    SELECT
        CustomerID,
        CustomerType,
        CustomerName,
        Phone,
        Email,
        Address,
        Country,
        IsActive,
        COUNT(*) OVER() AS TotalCount
    FROM Customers_View
    WHERE (@IsActive IS NULL OR IsActive = @IsActive)
      AND (
        @CustomerType IS NULL
        OR
        (
            (@CustomerType = 'Person' AND PersonID IS NOT NULL)
            OR
            (@CustomerType = 'Company' AND CompanyID IS NOT NULL)
        )
      )
      AND (
        @SearchText IS NULL OR @SearchText = ''
        OR
        (
            (@SearchBy = 'CustomerName' AND CustomerName LIKE '%' + @SearchText + '%')
            OR
            (@SearchBy = 'Phone' AND Phone LIKE '%' + @SearchText + '%')
            OR
            (@SearchBy = 'Code' AND CAST(CustomerID AS NVARCHAR(20)) LIKE '%' + @SearchText + '%')
        )
      )
    ORDER BY
        CASE WHEN @SortBy = 'CustomerID' THEN CustomerID END ASC,
        CASE WHEN @SortBy = 'CustomerName' THEN CustomerName END ASC,
        CASE WHEN @SortBy = 'Phone' THEN Phone END ASC,
        CASE WHEN @SortBy = 'Country' THEN Country END ASC,
        CustomerName ASC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO
