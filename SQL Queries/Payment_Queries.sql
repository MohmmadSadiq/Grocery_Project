USE [RMS];
GO

-- Create User-Defined Table Type for Payment Allocations
CREATE TYPE PaymentAllocationsType AS TABLE
(
    [TransactionID] [int] NOT NULL,
    [Amount] [decimal](18, 2) NOT NULL
)
GO

ALTER PROCEDURE spPayment_AddNew
    @PaymentDate DATETIME,
    @PaymentMethodID INT,
    @PaymentAmount DECIMAL(18,2),
    @Notes NVARCHAR(MAX),
    @CreatedByUserID INT,
    @NewAllocations PaymentAllocationsType READONLY,
    @NewPaymentID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION
    BEGIN TRY
        -- إضافة Payment جديد
        INSERT INTO Payments (
            PaymentDate, PaymentMethodID, PaymentAmount, Notes, CreatedByUserID
            , CreatedDate, IsDeleted 
        )
        VALUES (
            @PaymentDate, @PaymentMethodID, @PaymentAmount, @Notes, @CreatedByUserID
            , GETDATE(), 0 
        );

        -- الحصول على PaymentID الجديد
        SET @NewPaymentID = SCOPE_IDENTITY();

        -- إضافة السجلات إلى PaymentAllocations
        INSERT INTO PaymentAllocations
        (
            PaymentID, TransactionID, Amount, CreatedByUserID,
            CreatedDate, IsDeleted
        )
        SELECT
            @NewPaymentID, TransactionID, Amount, @CreatedByUserID,
            GETDATE(), 0
        FROM @NewAllocations;

        -- تأكيد العملية بنجاح
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- في حال حدث خطأ، التراجع عن كل العمليات
        ROLLBACK TRANSACTION;

        -- إعادة رفع الخطأ لإظهار التفاصيل للمستدعي
        THROW;
    END CATCH;
END
GO

ALTER PROCEDURE spPayment_GetAll AS
BEGIN
    SET NOCOUNT ON;

    SELECT PaymentID, PaymentDate, PaymentMethodID, PaymentAmount, Notes, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID FROM Payments WHERE IsDeleted = 0;
END
GO

ALTER PROCEDURE spPayment_GetByID
    @PaymentID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Return Payment Header
    SELECT PaymentID, PaymentDate, PaymentMethodID, PaymentAmount, Notes, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID 
    FROM Payments 
    WHERE PaymentID = @PaymentID AND IsDeleted = 0;

    -- Return Payment Allocations
    SELECT AllocationID, PaymentID, TransactionID, Amount, CreatedDate, CreatedByUserID
    FROM PaymentAllocations
    WHERE PaymentID = @PaymentID AND IsDeleted = 0;
END
GO

ALTER PROCEDURE spPayment_Update
    @PaymentID INT,
    @PaymentDate DATETIME,
    @PaymentMethodID INT,
    @PaymentAmount DECIMAL(18,2),
    @Notes NVARCHAR(MAX),
    @UpdatedByUserID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Payments SET
        PaymentDate = @PaymentDate,
        PaymentMethodID = @PaymentMethodID,
        PaymentAmount = @PaymentAmount,
        Notes = @Notes,
        UpdatedByUserID = @UpdatedByUserID,
        UpdatedDate = GETDATE()
    WHERE PaymentID = @PaymentID AND IsDeleted = 0;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

ALTER PROCEDURE spPayment_Delete
    @PaymentID INT,
    @UpdatedByUserID INT
AS 
BEGIN
     SET NOCOUNT ON;
    DECLARE @IsCompleted INT;

    BEGIN TRY
        BEGIN TRANSACTION

        -- Attempt to delete (Intercepted by Trigger)
        DELETE FROM Payments
        WHERE PaymentID = @PaymentID AND IsDeleted != 1

        SET @IsCompleted = @@ROWCOUNT;
        -- If ID didn't exist or was already deleted
        IF @@ROWCOUNT = 0
            THROW 51000, 'No record found to delete', 1;

        -- Update audit info
        UPDATE Payments
        SET UpdatedByUserID = @UpdatedByUserID,
            UpdatedDate = GETDATE()
        WHERE PaymentID = @PaymentID

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
    END CATCH

    IF @IsCompleted > 0 RETURN 1; ELSE RETURN 0;
END
GO

CREATE TRIGGER PaymentSoftDelete
    ON Payments
    INSTEAD OF DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE Payments
    SET IsDeleted = 1
    WHERE PaymentID IN (SELECT PaymentID FROM deleted)
      AND IsDeleted != 1
END

-- Example Usage:
/*
-- إنشاء جدول مؤقت للـ Allocations
DECLARE @Allocations PaymentAllocationsType;

-- إضافة سجلات للتوزيعات
INSERT INTO @Allocations (TransactionID, Amount)
VALUES 
    (1, 500.00),
    (2, 300.00),
    (3, 200.00);

-- استدعاء stored procedure لإضافة Payment مع Allocations
DECLARE @NewPaymentID INT;

EXEC spPayment_AddNew
    @PaymentDate = '2026-02-09',
    @PaymentMethodID = 1,
    @PaymentAmount = 1000.00,
    @Notes = 'دفعة من المورد',
    @CreatedByUserID = 1,
    @NewAllocations = @Allocations,
    @NewPaymentID = @NewPaymentID OUTPUT;

SELECT @NewPaymentID AS NewPaymentID;
*/
