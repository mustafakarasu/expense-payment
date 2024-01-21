CREATE PROCEDURE dbo.sp_PaymentDensityForEmployee
@userId int,
@startingDate datetime2, 
@lastDate datetime2 = null
AS
BEGIN
	SELECT dbo.PAYMENT_EMPLOYEE(1,@userId) PaidAmount, 
	dbo.PAYMENT_EMPLOYEE(0,@userId) RejectedAmount, 
	dbo.PAYMENTPENDING_EMPLOYEE(@userId) PendingAmount,
	@startingDate StartingDate,
	@lastDate LastDate
	FROM Payments p
	WHERE p.CreatedDate BETWEEN @startingDate AND @lastDate
END