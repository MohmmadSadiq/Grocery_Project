USE [RMS];
GO

ALTER PROCEDURE spBatch_AddNew
    @PurchaseID INT,
    @ProductUnitID INT,
    @TotalQuantity DECIMAL(18,4),
    @UniteCostPrice DECIMAL(18,2),
    @ProductionDate DATE,
    @ExpiryDate DATE,
    @BatchNumber NVARCHAR(50),
    @NewBatchID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO PurchaseProductBatches (
        PurchaseID, ProductUnitID, TotalQuantity, UniteCostPrice, ProductionDate, ExpiryDate, BatchNumber, IsDeleted
         
    )
    VALUES (
        @PurchaseID, @ProductUnitID, @TotalQuantity, @UniteCostPrice, @ProductionDate, @ExpiryDate, @BatchNumber, 0
         
    );

    SET @NewBatchID = SCOPE_IDENTITY();
END
GO



ALTER PROCEDURE spBatch_GetAll AS
BEGIN
    SET NOCOUNT ON;

    SELECT BatchID, PurchaseID, ProductUnitID, TotalQuantity, UniteCostPrice, ProductionDate, ExpiryDate, BatchNumber FROM PurchaseProductBatches WHERE IsDeleted = 0;
END
GO

ALTER PROCEDURE spBatch_GetByID
    @BatchID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT BatchID, PurchaseID, ProductUnitID, TotalQuantity, UniteCostPrice, ProductionDate, ExpiryDate, BatchNumber FROM PurchaseProductBatches WHERE BatchID = @BatchID AND IsDeleted = 0;
END
GO

ALTER PROCEDURE spBatch_Update
    @BatchID INT,
    @PurchaseID INT,
    @ProductUnitID INT,
    @TotalQuantity DECIMAL(18,4),
    @UniteCostPrice DECIMAL(18,2),
    @ProductionDate DATE,
    @ExpiryDate DATE,
    @BatchNumber NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE PurchaseProductBatches SET
        PurchaseID = @PurchaseID,
        ProductUnitID = @ProductUnitID,
        TotalQuantity = @TotalQuantity,
        UniteCostPrice = @UniteCostPrice,
        ProductionDate = @ProductionDate,
        ExpiryDate = @ExpiryDate,
        BatchNumber = @BatchNumber
    WHERE BatchID = @BatchID AND IsDeleted = 0;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

ALTER PROCEDURE spBatch_Delete
    @BatchID INT
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE PurchaseProductBatches SET IsDeleted = 1 WHERE BatchID = @BatchID AND IsDeleted = 0;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

