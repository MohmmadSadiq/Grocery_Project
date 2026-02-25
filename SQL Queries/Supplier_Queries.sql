USE [RMS];
GO

CREATE PROCEDURE spSupplier_AddNew
    @PersonID INT,
    @CompanyID INT,
    @AccountID INT,
    @IsActive BIT,
    @CreatedByUserID INT,
    @NewSupplierID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Suppliers (
        PersonID, CompanyID, AccountID, IsActive, CreatedByUserID
        , CreatedDate , IsDeleted
    )
    VALUES (
        @PersonID, @CompanyID, @AccountID, @IsActive, @CreatedByUserID
        , GETDATE() , 0
    );

    SET @NewSupplierID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE spSupplier_GetAll AS
BEGIN
    SET NOCOUNT ON;

    SELECT SupplierID, PersonID, CompanyID, AccountID, IsActive, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID FROM Suppliers WHERE IsDeleted = 0;
END
GO

CREATE PROCEDURE spSupplier_GetByID
    @SupplierID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT SupplierID, PersonID, CompanyID, AccountID, IsActive, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID FROM Suppliers WHERE SupplierID = @SupplierID AND IsDeleted = 0;
END
GO

CREATE PROCEDURE spSupplier_Update
    @SupplierID INT,
    @PersonID INT,
    @CompanyID INT,
    @AccountID INT,
    @IsActive BIT,
    @UpdatedByUserID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Suppliers SET
        PersonID = @PersonID,
        CompanyID = @CompanyID,
        AccountID = @AccountID,
        IsActive = @IsActive,
        UpdatedByUserID = @UpdatedByUserID
,
        UpdatedDate = GETDATE()
    WHERE SupplierID = @SupplierID;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

CREATE PROCEDURE spSupplier_Delete
    @SupplierID INT,
    @UpdatedByUserID INT

AS 
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsCompleted INT;

    BEGIN TRY
        BEGIN TRANSACTION

        -- Attempt to delete (Intercepted by Trigger)
        DELETE FROM Suppliers
        WHERE SupplierID = @SupplierID AND IsDeleted != 1

        SET @IsCompleted = @@ROWCOUNT;
        -- If ID didn't exist or was already deleted
        IF @@ROWCOUNT = 0
            THROW 51000, 'No record found to delete', 1;

        -- Update audit info
        UPDATE Suppliers
        SET UpdatedByUserID = @UpdatedByUserID,
            UpdatedDate = GETDATE()
        WHERE SupplierID = @SupplierID

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
    END CATCH

    IF @IsCompleted > 0 RETURN 1; ELSE RETURN 0;
END
GO

CREATE TRIGGER SupplierSoftDelete
    ON Suppliers
    INSTEAD OF DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE Suppliers
    SET IsDeleted = 1
    WHERE SupplierID IN (SELECT SupplierID FROM deleted)
      AND IsDeleted != 1
END
GO

ALTER TABLE Suppliers
ADD CONSTRAINT CK_Suppliers_PersonOrCompany
CHECK (
    (PersonID IS NOT NULL AND CompanyID IS NULL)
    OR (PersonID IS NULL AND CompanyID IS NOT NULL)
);
go

Alter View Suppliers_View as
SELECT 
    Suppliers.SupplierID, SupplierType = 'Company', 
	Companies.CompanyName AS SupplierName,
    Companies.Phone AS Phone,
	Companies.Email AS Email,
    Companies.Address AS Address,
    
   (select CountryName from Countries
	Where CountryID = Companies.CountryID) As Country 
	,
	IsActive
    ,Suppliers.CompanyID
	,Suppliers.PersonID
    
FROM Companies
 LEFT JOIN People ON Companies.ContactPersonID = People.PersonID
 JOIN Suppliers ON (Companies.CompanyID = Suppliers.CompanyID )
 WHERE Suppliers.IsDeleted = 0 
 Union
 SELECT 
    Suppliers.SupplierID, SupplierType = 'Person', 
	People.FullName AS SupplierName,
    People.Phone AS Phone,
	People.Email AS Email,
    People.Address AS Address,
    
    -- Country ID based on type
   (select CountryName from Countries
	Where CountryID = People.NationalityCountryID ) As Country 
	,
	IsActive
    ,Suppliers.CompanyID
	,Suppliers.PersonID

FROM  Suppliers
JOIN People on Suppliers.PersonID = People.PersonID

WHERE Suppliers.IsDeleted = 0 


GO
CREATE OR ALTER PROCEDURE sp_SearchSupplierPages
    @SearchText NVARCHAR(100) = NULL,
    @SearchBy   NVARCHAR(50) = 'SupplierName',  -- SupplierName, Phone, Code (SupplierID)
    @SupplierType NVARCHAR(20) = NULL,  -- 'Person', 'Company', NULL for all
    @IsActive   BIT = NULL,
    @PageNumber INT = 1,
    @PageSize   INT = 20,
    @SortBy     NVARCHAR(50) = 'SupplierName' -- SupplierName, Phone, Code, Country
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

	IF @SortBy NOT IN ('SupplierID','SupplierName','Phone','Country')
    SET @SortBy = 'SupplierName';
	IF @PageNumber < 1 SET @PageNumber = 1;

    SELECT 
        SupplierID,
        SupplierType,
        SupplierName,
        Phone,
        Email,
        Address,
        Country,
        IsActive,
        COUNT(*) OVER() AS TotalCount
    FROM Suppliers_View 
    WHERE 
        (@IsActive IS NULL OR IsActive = @IsActive)
        AND (@SupplierType IS NULL OR
		  (
        (@SupplierType = 'Person'  AND PersonID  IS NOT NULL)
        OR
        (@SupplierType = 'Company' AND CompanyID IS NOT NULL)
		)
		)
        AND (@SearchText IS NULL OR @SearchText = '' OR (
            (@SearchBy = 'SupplierName' AND SupplierName LIKE '%' + @SearchText + '%')
            OR (@SearchBy = 'Phone' AND Phone LIKE '%' + @SearchText + '%')
            OR (@SearchBy = 'Code' AND CAST(SupplierID AS NVARCHAR(20)) LIKE '%' + @SearchText + '%')
        ))
    ORDER BY 
        CASE WHEN @SortBy = 'SupplierID' THEN SupplierID END ASC,
        CASE WHEN @SortBy = 'SupplierName' THEN SupplierName END ASC,
        CASE WHEN @SortBy = 'Phone' THEN Phone END ASC,
        CASE WHEN @SortBy = 'Country' THEN Country END ASC,
        SupplierName ASC
		-- Default secondary sort
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END
