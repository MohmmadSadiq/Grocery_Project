USE [RMS];
GO

-- =============================================
-- TVP: ProductSalesType
-- Description: Table-Valued Parameter to pass sale line-items in a single round-trip.
-- =============================================
IF TYPE_ID('dbo.ProductSalesType') IS NOT NULL
    DROP TYPE dbo.ProductSalesType;
GO

CREATE TYPE dbo.ProductSalesType AS TABLE
(
    [ProductUnitID] [int]            NOT NULL,
    [Quantity]      [decimal](18, 4) NOT NULL,
    [UnitPrice]     [decimal](18, 2) NOT NULL
);
GO

-- =============================================
-- View: SalesGrid_View
-- Description: Flattened view used by GetAll and Search procedures.
-- =============================================
IF OBJECT_ID('dbo.SalesGrid_View', 'V') IS NOT NULL
    DROP VIEW dbo.SalesGrid_View;
GO

CREATE VIEW dbo.SalesGrid_View
AS
SELECT
    S.SaleID,
    S.CustomerID,
    T.TransactionID,
    T.PaymentID,
    T.TransactionDate,
    T.TransactionStatus,
    T.TotalAmount,
    T.Nots,
    T.CreatedDate,
    T.CreatedByUserID,
    T.UpdatedDate,
    T.UpdatedByUserID,
    T.IsDeleted,
    -- Customer name (Person full name or Company name, whichever exists)
    COALESCE(P.FullName, Co.CompanyName, N'عميل نقدي') AS CustomerName,
    CASE
        WHEN C.PersonID  IS NOT NULL THEN N'Person'
        WHEN C.CompanyID IS NOT NULL THEN N'Company'
        ELSE NULL
    END AS CustomerType,
    (SELECT SUM(Amount) FROM PaymentAllocations WHERE PaymentAllocations.TransactionID = S.TransactionID) AS PaidAmount
FROM Sales S
INNER JOIN Transactions T ON S.TransactionID = T.TransactionID
LEFT  JOIN Customers    C  ON S.CustomerID    = C.CustomerID
LEFT  JOIN People       P  ON C.PersonID      = P.PersonID
LEFT  JOIN Companies    Co ON C.CompanyID      = Co.CompanyID;
GO

-- =============================================
-- SP: spSales_AddNew
-- Description: Inserts a Sale header + detail lines inside a transaction.
-- =============================================
IF OBJECT_ID('dbo.spSales_AddNew', 'P') IS NOT NULL
    DROP PROCEDURE dbo.spSales_AddNew;
GO

CREATE PROCEDURE dbo.spSales_AddNew
    @TransactionID INT,
    @CustomerID    INT           = NULL,
    @SaleItems     ProductSalesType READONLY,
    @NewSaleID     INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. Insert Sale header
        INSERT INTO Sales (TransactionID, CustomerID)
        VALUES (@TransactionID, @CustomerID);

        SET @NewSaleID = SCOPE_IDENTITY();

        -- 2. Insert detail lines from TVP
        INSERT INTO ProductSales (SaleID, ProductUnitID, Quantity, UnitPrice)
        SELECT @NewSaleID, ProductUnitID, Quantity, UnitPrice
        FROM @SaleItems;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- =============================================
-- SP: spSales_GetByID
-- Description: Returns Sale header (result-set 1) + ProductSales details (result-set 2).
-- =============================================
IF OBJECT_ID('dbo.spSales_GetByID', 'P') IS NOT NULL
    DROP PROCEDURE dbo.spSales_GetByID;
GO

CREATE PROCEDURE dbo.spSales_GetByID
    @SaleID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Result Set 1: Header
    SELECT
        S.SaleID,
        S.TransactionID,
        S.CustomerID
    FROM Sales S
    INNER JOIN Transactions T
        ON T.TransactionID = S.TransactionID
       AND T.IsDeleted = 0
    WHERE S.SaleID = @SaleID;

    -- Result Set 2: Details
    SELECT
        PS.ProductSaleID,
        PS.SaleID,
        PS.ProductUnitID,
        PS.Quantity,
        PS.UnitPrice,
        PS.TotalPrice
    FROM ProductSales PS
    INNER JOIN Sales S
        ON PS.SaleID = S.SaleID
    INNER JOIN Transactions T
        ON T.TransactionID = S.TransactionID
       AND T.IsDeleted = 0
    WHERE PS.SaleID = @SaleID;
END;
GO

-- =============================================
-- SP: spSales_Update
-- Description: Updates the Sale header fields.
-- =============================================
IF OBJECT_ID('dbo.spSales_Update', 'P') IS NOT NULL
    DROP PROCEDURE dbo.spSales_Update;
GO

CREATE PROCEDURE dbo.spSales_Update
    @SaleID        INT,
    @TransactionID INT,
    @CustomerID    INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Sales SET
        TransactionID = @TransactionID,
        CustomerID    = @CustomerID
    WHERE SaleID = @SaleID;

    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0;
END;
GO

-- =============================================
-- SP: spSales_Delete
-- Description: Soft-deletes the Sale by delegating to spTransaction_Delete.
-- =============================================
IF OBJECT_ID('dbo.spSales_Delete', 'P') IS NOT NULL
    DROP PROCEDURE dbo.spSales_Delete;
GO

CREATE PROCEDURE dbo.spSales_Delete
    @SaleID         INT,
    @UpdatedByUserID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TransactionID INT = (SELECT TransactionID FROM Sales WHERE SaleID = @SaleID);

    IF @TransactionID IS NULL
        RETURN -1;

    DECLARE @Result INT;

    EXEC @Result = spTransaction_Delete @TransactionID, @UpdatedByUserID;

    RETURN @Result;
END;
GO

-- =============================================
-- SP: spSales_GetAll
-- Description: Returns all sales from the grid view.
-- =============================================
IF OBJECT_ID('dbo.spSales_GetAll', 'P') IS NOT NULL
    DROP PROCEDURE dbo.spSales_GetAll;
GO

CREATE PROCEDURE dbo.spSales_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM SalesGrid_View
    WHERE IsDeleted = 0;
END;
GO

-- =============================================
-- SP: sp_SearchSalesPages
-- Description: Paged search with filtering. Mirrors sp_SearchPurchasePages pattern.
-- =============================================
IF OBJECT_ID('dbo.sp_SearchSalesPages', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_SearchSalesPages;
GO

CREATE PROCEDURE dbo.sp_SearchSalesPages
    @SearchText        NVARCHAR(100) = NULL,
    @SearchBy          NVARCHAR(50)  = 'SaleID',         -- SaleID, CustomerName
    @TransactionStatus TINYINT       = NULL,              -- 1=InProgress, 2=Cancelled, 3=Completed, NULL=All
    @CustomerType      NVARCHAR(20)  = NULL,              -- 'Person', 'Company', NULL for all
    @PageNumber        INT           = 1,
    @PageSize          INT           = 20,
    @SortBy            NVARCHAR(50)  = 'TransactionDate'  -- TransactionDate, SaleID, CustomerName, TotalAmount, PaidAmount
AS
BEGIN
    SET NOCOUNT ON;
    SET ARITHABORT ON;

    -- حل Parameter Sniffing
    DECLARE @LocalSearchText        NVARCHAR(100) = @SearchText;
    DECLARE @LocalSearchBy          NVARCHAR(50)  = @SearchBy;
    DECLARE @LocalTransactionStatus TINYINT       = @TransactionStatus;
    DECLARE @LocalCustomerType      NVARCHAR(20)  = @CustomerType;
    DECLARE @LocalPageNumber        INT           = @PageNumber;
    DECLARE @LocalPageSize          INT           = @PageSize;
    DECLARE @LocalSortBy            NVARCHAR(50)  = @SortBy;

    DECLARE @Offset INT = (@LocalPageNumber - 1) * @LocalPageSize;

    IF @LocalSearchText = '' SET @LocalSearchText = NULL;

    -- حساب TotalCount منفصل لتحسين الأداء
    DECLARE @TotalCount INT;

    SELECT @TotalCount = COUNT(*)
    FROM SalesGrid_View
    WHERE
        IsDeleted = 0
        AND (@LocalTransactionStatus IS NULL OR TransactionStatus = @LocalTransactionStatus)
        AND (@LocalCustomerType IS NULL OR CustomerType = @LocalCustomerType)
        AND (@LocalSearchText IS NULL OR (
            (@LocalSearchBy = 'SaleID'       AND CAST(SaleID AS NVARCHAR(20)) LIKE '%' + @LocalSearchText + '%')
            OR (@LocalSearchBy = 'CustomerName' AND CustomerName LIKE '%' + @LocalSearchText + '%')
        ))
    OPTION (RECOMPILE);

    -- الاستعلام الرئيسي
    SELECT
        SaleID,
        CustomerID,
        CustomerName,
        CustomerType,
        TransactionDate,
        TotalAmount,
        PaidAmount,
        TransactionStatus,
        @TotalCount AS TotalCount
    FROM SalesGrid_View
    WHERE
        IsDeleted = 0
        AND (@LocalTransactionStatus IS NULL OR TransactionStatus = @LocalTransactionStatus)
        AND (@LocalCustomerType IS NULL OR CustomerType = @LocalCustomerType)
        AND (@LocalSearchText IS NULL OR (
            (@LocalSearchBy = 'SaleID'       AND CAST(SaleID AS NVARCHAR(20)) LIKE '%' + @LocalSearchText + '%')
            OR (@LocalSearchBy = 'CustomerName' AND CustomerName LIKE '%' + @LocalSearchText + '%')
        ))
    ORDER BY
        CASE WHEN @LocalSortBy = 'SaleID'          THEN SaleID END DESC,
        CASE WHEN @LocalSortBy = 'CustomerName'     THEN CustomerName END ASC,
        CASE WHEN @LocalSortBy = 'TotalAmount'      THEN TotalAmount END DESC,
        CASE WHEN @LocalSortBy = 'PaidAmount'       THEN PaidAmount END DESC,
        CASE WHEN @LocalSortBy = 'TransactionDate'  THEN TransactionDate END DESC,
        TransactionDate DESC
    OFFSET @Offset ROWS FETCH NEXT @LocalPageSize ROWS ONLY
    OPTION (RECOMPILE);
END;
GO
