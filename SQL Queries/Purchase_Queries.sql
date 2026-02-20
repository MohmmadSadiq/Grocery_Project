USE [RMS];
GO

CREATE PROCEDURE spPurchase_AddNew
    @TransactionID INT,
    @SupplierID INT,
    @InvoiceNumber NVARCHAR(50),
    @PurchasedByEmployeeID INT,
	@NewBatches PurchaseProductBatchesType READONLY,
    @NewPurchaseID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

	
	BEGIN TRANSACTION
		BEGIN TRY
		    -- ≈œŒ«· «·”Ã· «·—∆Ì”Ì ›Ì ÃœÊ· Purchases
		    INSERT INTO Purchases (
		        TransactionID, SupplierID, InvoiceNumber, PurchasedByEmployeeID
		    )
		    VALUES (
		        @TransactionID, @SupplierID, @InvoiceNumber, @PurchasedByEmployeeID
		    );
		
		    -- «·Õ’Ê· ⁄·Ï «·‹ PurchaseID «·ÃœÌœ
		    SET @NewPurchaseID = SCOPE_IDENTITY();
		
		    -- ≈œŒ«· «·»« ‘«  «·„— »ÿ… »«·‘—«¡
		    INSERT INTO PurchaseProductBatches
		    (
		        PurchaseID, ProductUnitID, TotalQuantity, UniteCostPrice,
		        ProductionDate, ExpiryDate, BatchNumber
		    )
		    SELECT
		        @NewPurchaseID, ProductUnitID, TotalQuantity, 
		        UniteCostPrice, ProductionDate, ExpiryDate, BatchNumber
		    FROM @NewBatches;  -- <-- Â‰« «” Œœ„‰« «·«”„ «·ÃœÌœ ··„ €Ì—
		
		    -- ≈‰Â«¡ «·⁄„·Ì… »‰Ã«Õ
		    COMMIT TRANSACTION;
		END TRY
		BEGIN CATCH
		    -- ≈–« Õ’· Œÿ√° «· —«Ã⁄ ⁄‰ ﬂ· «· €ÌÌ—« 
		    ROLLBACK TRANSACTION;
		
		    -- —›⁄ «·Œÿ√ ··Œ«—Ã ·Ì „ «· ⁄«„· „⁄Â
		    THROW;
		END CATCH;
		
END
GO

CREATE PROCEDURE spPurchase_GetAll AS
BEGIN
    SET NOCOUNT ON;

	SELECT * FROM Purchase_View

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
        P.PurchasedByEmployeeID
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

CREATE PROCEDURE spPurchase_Update
    @PurchaseID INT,
    @TransactionID INT,
    @SupplierID INT,
    @InvoiceNumber NVARCHAR(50),
    @PurchasedByEmployeeID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Purchases SET
        TransactionID = @TransactionID,
        SupplierID = @SupplierID,
        InvoiceNumber = @InvoiceNumber,
        PurchasedByEmployeeID = @PurchasedByEmployeeID
    WHERE PurchaseID = @PurchaseID;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

CREATE PROCEDURE spPurchase_Delete
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
                         Transactions.CreatedByUserID, Transactions.UpdatedDate, Transactions.UpdatedByUserID, Purchases.SupplierID, Purchases.InvoiceNumber, Purchases.PurchasedByEmployeeID, IsDeleted
FROM            Purchases INNER JOIN
                         Transactions ON Purchases.TransactionID = Transactions.TransactionID

SELECT * FROM Purchase_View


select * from PurchaseProductBatches



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



