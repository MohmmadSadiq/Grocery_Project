USE [RMS];
GO

CREATE PROCEDURE spTransaction_AddNew
    @PaymentID INT,
    @TransactionDate DATETIME,
    @TransactionType TINYINT,
    @TransactionStatus TINYINT,
    @TotalAmount DECIMAL(18,2),
    @Nots NVARCHAR,
    @CreatedByUserID INT,
    @NewTransactionID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Transactions (
        PaymentID, TransactionDate, TransactionType, TransactionStatus, TotalAmount, Nots, CreatedByUserID
        , CreatedDate , IsDeleted
    )
    VALUES (
        @PaymentID, @TransactionDate, @TransactionType, @TransactionStatus, @TotalAmount, @Nots, @CreatedByUserID
        , GETDATE() , 0
    );

    SET @NewTransactionID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE spTransaction_GetAll AS
BEGIN
    SET NOCOUNT ON;

    SELECT TransactionID, PaymentID, TransactionDate, TransactionType, TransactionStatus, TotalAmount, Nots, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID FROM Transactions WHERE IsDeleted = 0;
END
GO

CREATE PROCEDURE spTransaction_GetByID
    @TransactionID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TransactionID, PaymentID, TransactionDate, TransactionType, TransactionStatus, TotalAmount, Nots, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID FROM Transactions WHERE TransactionID = @TransactionID AND IsDeleted = 0;
END
GO

CREATE PROCEDURE spTransaction_Update
    @TransactionID INT,
    @PaymentID INT,
    @TransactionDate DATETIME,
    @TransactionType TINYINT,
    @TransactionStatus TINYINT,
    @TotalAmount DECIMAL(18,2),
    @Nots NVARCHAR,
    @UpdatedByUserID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Transactions SET
        PaymentID = @PaymentID,
        TransactionDate = @TransactionDate,
        TransactionType = @TransactionType,
        TransactionStatus = @TransactionStatus,
        TotalAmount = @TotalAmount,
        Nots = @Nots,
        UpdatedByUserID = @UpdatedByUserID
,
        UpdatedDate = GETDATE()
    WHERE TransactionID = @TransactionID;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

CREATE PROCEDURE spTransaction_Delete
    @TransactionID INT,
    @UpdatedByUserID INT

AS 
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsCompleted INT;

    BEGIN TRY
        BEGIN TRANSACTION

        -- Attempt to delete (Intercepted by Trigger)
        DELETE FROM Transactions
        WHERE TransactionID = @TransactionID AND IsDeleted != 1

        SET @IsCompleted = @@ROWCOUNT;
        -- If ID didn't exist or was already deleted
        IF @@ROWCOUNT = 0
            THROW 51000, 'No record found to delete', 1;

        -- Update audit info
        UPDATE Transactions
        SET UpdatedByUserID = @UpdatedByUserID,
            UpdatedDate = GETDATE()
        WHERE TransactionID = @TransactionID

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
    END CATCH

    IF @IsCompleted > 0 RETURN 1; ELSE RETURN 0;
END
GO

CREATE TRIGGER TransactionSoftDelete
    ON Transactions
    INSTEAD OF DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE Transactions
    SET IsDeleted = 1
    WHERE TransactionID IN (SELECT TransactionID FROM deleted)
      AND IsDeleted != 1
END
GO


