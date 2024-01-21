CREATE FUNCTION fn_PendingPayment(@userId int = null)
RETURNS DECIMAL(18,2)
AS BEGIN
    DECLARE @return decimal(18,2)

	IF @userId IS NULL
		BEGIN
			SET @return = (SELECT SUM(e.Amount) 
						    FROM Expenses e
							WHERE Id NOT IN (SELECT p.ExpenseId FROM Payments p))
		END
	ELSE
		BEGIN
			SET @return = (SELECT SUM(e.Amount)
						   FROM Expenses e
						   WHERE Id NOT IN (SELECT p.ExpenseId FROM Payments p) AND e.UserId = @userId)
		END


    IF @return is null
		SET @return = 0

    RETURN @return
END