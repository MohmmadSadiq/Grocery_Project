USE [RMS];
GO

CREATE PROCEDURE spPaymentMethod_AddNew
    @MethodName NVARCHAR(50),
    @Description NVARCHAR(MAX),
    @IsActiveForSales BIT,
    @IsActiveForPurchases BIT,
    @NewPaymentMethodID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO PaymentMethods (
        MethodName, Description, IsActiveForSales, IsActiveForPurchases
         
    )
    VALUES (
        @MethodName, @Description, @IsActiveForSales, @IsActiveForPurchases
         
    );

    SET @NewPaymentMethodID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE spPaymentMethod_GetAll AS
BEGIN
    SET NOCOUNT ON;

    SELECT PaymentMethodID, MethodName, Description, IsActiveForSales, IsActiveForPurchases FROM PaymentMethods ;
END
GO

CREATE PROCEDURE spPaymentMethod_GetByID
    @PaymentMethodID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT PaymentMethodID, MethodName, Description, IsActiveForSales, IsActiveForPurchases FROM PaymentMethods WHERE PaymentMethodID = @PaymentMethodID ;
END
GO

CREATE PROCEDURE spPaymentMethod_Update
    @PaymentMethodID INT,
    @MethodName NVARCHAR(50),
    @Description NVARCHAR(MAX),
    @IsActiveForSales BIT,
    @IsActiveForPurchases BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE PaymentMethods SET
        MethodName = @MethodName,
        Description = @Description,
        IsActiveForSales = @IsActiveForSales,
        IsActiveForPurchases = @IsActiveForPurchases
    WHERE PaymentMethodID = @PaymentMethodID;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

CREATE PROCEDURE spPaymentMethod_Delete
    @PaymentMethodID INT
AS 
BEGIN
    SET NOCOUNT ON;
    DELETE FROM PaymentMethods WHERE PaymentMethodID = @PaymentMethodID;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

