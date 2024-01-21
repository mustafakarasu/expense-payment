CREATE PROCEDURE dbo.sp_PaymentDensityForCompany
@startingDate datetime2, 
@lastDate datetime2 = null
AS
BEGIN
	SELECT dbo.fn_Payment(1, null) PaidAmount, 
	dbo.fn_Payment(0, null) RejectedAmount, 
	dbo.fn_PendingPayment(null) PendingAmount,
	@startingDate StartingDate,
	@lastDate LastDate
	FROM Payments p
	WHERE p.CreatedDate BETWEEN @startingDate AND @lastDate
END