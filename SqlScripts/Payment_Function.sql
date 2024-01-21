CREATE FUNCTION fn_Payment (@isApproved bit = 1, @userId int = null)
RETURNS DECIMAL(18,2)
AS BEGIN
    DECLARE @return decimal(18,2)

	IF @userId IS NULL
		BEGIN
			SET @return = (SELECT SUM(e.Amount) as PaidAmount
							FROM Payments p
							INNER JOIN Expenses e on p.ExpenseId = e.Id
							WHERE p.IsApproved = @isApproved)
		END
	ELSE
		BEGIN
			SET @return = (SELECT SUM(e.Amount) as PaidAmount
							FROM Payments p
							INNER JOIN Expenses e on p.ExpenseId = e.Id
							WHERE p.IsApproved = @isApproved AND e.UserId = @userId)
		END
	
	IF @return is null
		SET @return = 0

    RETURN @return
END