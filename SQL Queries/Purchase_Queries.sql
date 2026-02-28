USE [RMS];
GO

ALTER PROCEDURE spPurchase_AddNew
    @TransactionID INT,
    @SupplierID INT,
    @InvoiceNumber NVARCHAR(50),
    @PurchasedByEmployeeID INT,
	@InvoiceDocumentPath NVARCHAR(500) = NULL,
	@NewBatches PurchaseProductBatchesType READONLY,
    @NewPurchaseID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

	
	BEGIN TRANSACTION
		BEGIN TRY
		    INSERT INTO Purchases (
		        TransactionID, SupplierID, InvoiceNumber, PurchasedByEmployeeID, InvoiceDocumentPath
		    )
		    VALUES (
		        @TransactionID, @SupplierID, @InvoiceNumber, @PurchasedByEmployeeID, @InvoiceDocumentPath
		    );
		
		    -- ������ ��� ��� PurchaseID ������
		    SET @NewPurchaseID = SCOPE_IDENTITY();
		
		    -- ����� �������� �������� �������
		    INSERT INTO PurchaseProductBatches
		    (
		        PurchaseID, ProductUnitID, TotalQuantity, UniteCostPrice,
		        ProductionDate, ExpiryDate, BatchNumber
		    )
		    SELECT
		        @NewPurchaseID, ProductUnitID, TotalQuantity, 
		        UniteCostPrice, ProductionDate, ExpiryDate, BatchNumber
		    FROM @NewBatches;  -- <-- ��� �������� ����� ������ �������
		
		    -- ����� ������� �����
		    COMMIT TRANSACTION;
		END TRY
		BEGIN CATCH
		    -- ��� ��� ��á ������� �� �� ���������
		    ROLLBACK TRANSACTION;
		
		    -- ��� ����� ������ ���� ������� ���
		    THROW;
		END CATCH;
		
END
GO

ALTER PROCEDURE spPurchase_GetAll AS
BEGIN
    SET NOCOUNT ON;

	SELECT * FROM PurchasesGrid_View

END
GO

ALTER PROCEDURE spPurchase_GetByID
    @PurchaseID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Header
    SELECT 
        P.PurchaseID,
        P.TransactionID,
        P.SupplierID,
        P.InvoiceNumber,
        P.PurchasedByEmployeeID,
        P.InvoiceDocumentPath
    FROM Purchases P
    INNER JOIN Transactions T 
        ON T.TransactionID = P.TransactionID
       AND T.IsDeleted = 0
    WHERE P.PurchaseID = @PurchaseID;


    -- Details
    SELECT 
        PPB.*
    FROM PurchaseProductBatches PPB
    INNER JOIN Purchases P
        ON PPB.PurchaseID = P.PurchaseID
    INNER JOIN Transactions T
        ON T.TransactionID = P.TransactionID
       AND T.IsDeleted = 0
    WHERE PPB.PurchaseID = @PurchaseID;

END
GO

GO

ALTER PROCEDURE spPurchase_Update
    @PurchaseID INT,
    @TransactionID INT,
    @SupplierID INT,
    @InvoiceNumber NVARCHAR(50),
    @PurchasedByEmployeeID INT,
    @InvoiceDocumentPath NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Purchases SET
        TransactionID = @TransactionID,
        SupplierID = @SupplierID,
        InvoiceNumber = @InvoiceNumber,
        PurchasedByEmployeeID = @PurchasedByEmployeeID,
        InvoiceDocumentPath = @InvoiceDocumentPath
    WHERE PurchaseID = @PurchaseID;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

ALTER PROCEDURE spPurchase_Delete
    @PurchaseID INT,
	@UpdatedByUserID INT 
AS 
BEGIN

	DECLARE @TransactionID INT = (SELECT TransactionID FROM Purchases WHERE PurchaseID = @PurchaseID);

	IF @TransactionID IS NULL
        RETURN -1;

	DECLARE @Result INT;

	EXEC @Result =  spTransaction_Delete
	@TransactionID,
	@UpdatedByUserID
    
	RETURN @Result;
END
GO

ALTER VIEW Purchase_View
AS
SELECT        Transactions.TransactionID , Purchases.PurchaseID, Transactions.PaymentID, Transactions.TransactionDate, Transactions.TransactionType, Transactions.TransactionStatus, Transactions.TotalAmount, Transactions.Nots, Transactions.CreatedDate, 
                         Transactions.CreatedByUserID, Transactions.UpdatedDate, Transactions.UpdatedByUserID, Purchases.SupplierID, Purchases.InvoiceNumber, Purchases.PurchasedByEmployeeID, Purchases.InvoiceDocumentPath, IsDeleted
FROM            Purchases INNER JOIN
                         Transactions ON Purchases.TransactionID = Transactions.TransactionID

SELECT * FROM Purchase_View


select * from PurchaseProductBatches


x
CREATE TABLE [dbo].[PurchaseProductBatches](
	[BatchID] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseID] [int] NOT NULL,
	[ProductUnitID] [int] NOT NULL,
	[TotalQuantity] [decimal](18, 4) NOT NULL,
	[UniteCostPrice] [decimal](18, 2) NOT NULL,
	[TotalCostPrice]  AS ([TotalQuantity]*[UniteCostPrice]),
	[ProductionDate] [date] NULL,
	[ExpiryDate] [date] NULL,
	[BatchNumber] [nvarchar](50) NULL,
 CONSTRAINT [PK_PurchaseProductBatches] PRIMARY KEY CLUSTERED 
(
	[BatchID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TYPE PurchaseProductBatchesType AS TABLE
(
		[ProductUnitID] [int] NOT NULL,
		[TotalQuantity] [decimal](18, 4) NOT NULL,
		[UniteCostPrice] [decimal](18, 2) NOT NULL,
		[ProductionDate] [date] NULL,
		[ExpiryDate] [date] NULL,
		[BatchNumber] [nvarchar](50) NULL		 
)


ALTER TABLE Purchases
ADD InvoiceDocumentPath NVARCHAR(500) NULL;
select * from Purchases

/*
ALTER VIEW PurchasesGrid_View  as
SELECT        Purchases.PurchaseID, Purchases.SupplierID, Transactions.TransactionID, Purchases.InvoiceNumber, Transactions.PaymentID, Transactions.TransactionDate, Transactions.TransactionStatus, Transactions.TotalAmount 
                        , People.FullName as EmployeeName, Positions.PositionName ,(SELECT SUM(Amount) from PaymentAllocations where PaymentAllocations.TransactionID = Purchases.TransactionID) as PaidAmount, SupplierName, SupplierType
FROM            Transactions INNER JOIN
                         Purchases ON Transactions.TransactionID = Purchases.TransactionID
						 LEFT JOIN Employees ON Purchases.PurchasedByEmployeeID = Employees.EmployeeID
						 LEFT JOIN
                         People ON Employees.PersonID = People.PersonID LEFT JOIN
                         Positions ON Employees.PositionID = Positions.PositionID
						 join Suppliers_View on Purchases.SupplierID = Suppliers_View.SupplierID


ALTER PROCEDURE sp_SearchPurchasePages
    @SearchText        NVARCHAR(100) = NULL,
    @SearchBy          NVARCHAR(50)  = 'InvoiceNumber',  -- InvoiceNumber, PurchaseID, SupplierName, EmployeeName
    @TransactionStatus TINYINT       = NULL,              -- 1=InProgress, 2=Cancelled, 3=Completed, NULL=All
    @SupplierType      NVARCHAR(20)  = NULL,              -- 'Person', 'Company', NULL for all
    @PageNumber        INT           = 1,
    @PageSize          INT           = 20,
    @SortBy            NVARCHAR(50)  = 'TransactionDate'  -- TransactionDate, InvoiceNumber, SupplierName, TotalAmount, PurchaseID, EmployeeName, PaidAmount
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        PurchaseID,
        InvoiceNumber,
        SupplierName,
        SupplierType,
        TransactionDate,
        TotalAmount,
        PaidAmount,
        TransactionStatus,
        EmployeeName,
        PositionName,
        COUNT(*) OVER() AS TotalCount
    FROM PurchasesGrid_View
    WHERE 
        (@TransactionStatus IS NULL OR TransactionStatus = @TransactionStatus)
        AND (@SupplierType IS NULL OR SupplierType = @SupplierType)
        AND (@SearchText IS NULL OR @SearchText = '' OR (
            (@SearchBy = 'InvoiceNumber' AND InvoiceNumber LIKE '%' + @SearchText + '%')
            OR (@SearchBy = 'PurchaseID'    AND CAST(PurchaseID AS NVARCHAR(20)) LIKE '%' + @SearchText + '%')
            OR (@SearchBy = 'SupplierName'  AND SupplierName LIKE '%' + @SearchText + '%')
            OR (@SearchBy = 'EmployeeName'  AND EmployeeName LIKE '%' + @SearchText + '%')
        ))
    ORDER BY 
        CASE WHEN @SortBy = 'PurchaseID'      THEN PurchaseID END DESC,
        CASE WHEN @SortBy = 'InvoiceNumber'    THEN InvoiceNumber END ASC,
        CASE WHEN @SortBy = 'SupplierName'     THEN SupplierName END ASC,
        CASE WHEN @SortBy = 'TotalAmount'      THEN TotalAmount END DESC,
        CASE WHEN @SortBy = 'PaidAmount'       THEN PaidAmount END DESC,
        CASE WHEN @SortBy = 'EmployeeName'     THEN EmployeeName END ASC,
        CASE WHEN @SortBy = 'TransactionDate'  THEN TransactionDate END DESC,
        TransactionDate DESC  -- Default secondary sort
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

END*/

ALTER PROCEDURE sp_SearchPurchasePages
    @SearchText        NVARCHAR(100) = NULL,
    @SearchBy          NVARCHAR(50)  = 'InvoiceNumber',
    @TransactionStatus TINYINT       = NULL,
    @SupplierType      NVARCHAR(20)  = NULL,
    @PageNumber        INT           = 1,
    @PageSize          INT           = 20,
    @SortBy            NVARCHAR(50)  = 'TransactionDate'
AS
BEGIN
    SET NOCOUNT ON;
    SET ARITHABORT ON;

    -- حل Parameter Sniffing: نسخ المعاملات إلى متغيرات محلية
    DECLARE @LocalSearchText        NVARCHAR(100) = @SearchText;
    DECLARE @LocalSearchBy          NVARCHAR(50)  = @SearchBy;
    DECLARE @LocalTransactionStatus TINYINT       = @TransactionStatus;
    DECLARE @LocalSupplierType      NVARCHAR(20)  = @SupplierType;
    DECLARE @LocalPageNumber        INT           = @PageNumber;
    DECLARE @LocalPageSize          INT           = @PageSize;
    DECLARE @LocalSortBy            NVARCHAR(50)  = @SortBy;

    DECLARE @Offset INT = (@LocalPageNumber - 1) * @LocalPageSize;

    -- تنظيف النص: تحويل النص الفارغ إلى NULL لتبسيط الشرط
    IF @LocalSearchText = '' SET @LocalSearchText = NULL;

    -- حساب TotalCount بشكل منفصل لتحسين الأداء
    DECLARE @TotalCount INT;

    SELECT @TotalCount = COUNT(*)
    FROM PurchasesGrid_View
    WHERE 
        (@LocalTransactionStatus IS NULL OR TransactionStatus = @LocalTransactionStatus)
        AND (@LocalSupplierType IS NULL OR SupplierType = @LocalSupplierType)
        AND (@LocalSearchText IS NULL OR (
            (@LocalSearchBy = 'InvoiceNumber' AND InvoiceNumber LIKE '%' + @LocalSearchText + '%')
            OR (@LocalSearchBy = 'PurchaseID'    AND CAST(PurchaseID AS NVARCHAR(20)) LIKE '%' + @LocalSearchText + '%')
            OR (@LocalSearchBy = 'SupplierName'  AND SupplierName LIKE '%' + @LocalSearchText + '%')
            OR (@LocalSearchBy = 'EmployeeName'  AND EmployeeName LIKE '%' + @LocalSearchText + '%')
        ))
    OPTION (RECOMPILE);

    -- الاستعلام الرئيسي بدون COUNT(*) OVER() المكلف
    SELECT 
        PurchaseID,
        InvoiceNumber,
        SupplierName,
        SupplierType,
        TransactionDate,
        TotalAmount,
        PaidAmount,
        TransactionStatus,
        EmployeeName,
        PositionName,
        @TotalCount AS TotalCount
    FROM PurchasesGrid_View
    WHERE 
        (@LocalTransactionStatus IS NULL OR TransactionStatus = @LocalTransactionStatus)
        AND (@LocalSupplierType IS NULL OR SupplierType = @LocalSupplierType)
        AND (@LocalSearchText IS NULL OR (
            (@LocalSearchBy = 'InvoiceNumber' AND InvoiceNumber LIKE '%' + @LocalSearchText + '%')
            OR (@LocalSearchBy = 'PurchaseID'    AND CAST(PurchaseID AS NVARCHAR(20)) LIKE '%' + @LocalSearchText + '%')
            OR (@LocalSearchBy = 'SupplierName'  AND SupplierName LIKE '%' + @LocalSearchText + '%')
            OR (@LocalSearchBy = 'EmployeeName'  AND EmployeeName LIKE '%' + @LocalSearchText + '%')
        ))
    ORDER BY 
        CASE WHEN @LocalSortBy = 'PurchaseID'      THEN PurchaseID END DESC,
        CASE WHEN @LocalSortBy = 'InvoiceNumber'    THEN InvoiceNumber END ASC,
        CASE WHEN @LocalSortBy = 'SupplierName'     THEN SupplierName END ASC,
        CASE WHEN @LocalSortBy = 'TotalAmount'      THEN TotalAmount END DESC,
        CASE WHEN @LocalSortBy = 'PaidAmount'       THEN PaidAmount END DESC,
        CASE WHEN @LocalSortBy = 'EmployeeName'     THEN EmployeeName END ASC,
        CASE WHEN @LocalSortBy = 'TransactionDate'  THEN TransactionDate END DESC,
        TransactionDate DESC
    OFFSET @Offset ROWS FETCH NEXT @LocalPageSize ROWS ONLY
    OPTION (RECOMPILE);
END
