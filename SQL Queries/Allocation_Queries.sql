USE [RMS];
GO

CREATE PROCEDURE spAllocation_AddNew
    @PaymentID INT,
    @TransactionID INT,
    @Amount DECIMAL(18,2),
    @CreatedByUserID INT,
    @NewAllocationID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO PaymentAllocations (
        PaymentID, TransactionID, Amount, CreatedByUserID
        , CreatedDate , IsDeleted
    )
    VALUES (
        @PaymentID, @TransactionID, @Amount, @CreatedByUserID
        , GETDATE() , 0
    );

    SET @NewAllocationID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE spAllocation_GetAll AS
BEGIN
    SET NOCOUNT ON;

    SELECT AllocationID, PaymentID, TransactionID, Amount, CreatedDate, CreatedByUserID FROM PaymentAllocations WHERE IsDeleted = 0;
END
GO

CREATE PROCEDURE spAllocation_GetByID
    @AllocationID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT AllocationID, PaymentID, TransactionID, Amount, CreatedDate, CreatedByUserID FROM PaymentAllocations WHERE AllocationID = @AllocationID AND IsDeleted = 0;
END
GO

CREATE PROCEDURE spAllocation_Update
    @AllocationID INT,
    @PaymentID INT,
    @TransactionID INT,
    @Amount DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE PaymentAllocations SET
        PaymentID = @PaymentID,
        TransactionID = @TransactionID,
        Amount = @Amount
    WHERE AllocationID = @AllocationID;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

CREATE PROCEDURE spAllocation_Delete
    @AllocationID INT
AS 
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsCompleted INT;

    BEGIN TRY
        BEGIN TRANSACTION

        -- Attempt to delete (Intercepted by Trigger)
        DELETE FROM PaymentAllocations
        WHERE AllocationID = @AllocationID AND IsDeleted != 1

        SET @IsCompleted = @@ROWCOUNT;
        -- If ID didn't exist or was already deleted
        IF @@ROWCOUNT = 0
            THROW 51000, 'No record found to delete', 1;


        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
    END CATCH

    IF @IsCompleted > 0 RETURN 1; ELSE RETURN 0;
END
GO

CREATE TRIGGER AllocationSoftDelete
    ON PaymentAllocations
    INSTEAD OF DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE PaymentAllocations
    SET IsDeleted = 1
    WHERE AllocationID IN (SELECT AllocationID FROM deleted)
      AND IsDeleted != 1
END
GO

