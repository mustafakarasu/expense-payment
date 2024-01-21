namespace DataAccess.Migrations
{
    public class SqlExpressions
    {
        public const string FunctionPayment = """
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
                                              """;

        public const string FunctionPendingPayment = """
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
                                                     """;


        public const string StoredProcedureForCompany = """
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
                                                        """;

        public const string StoredProcedureForEmployee = """
                                                         CREATE PROCEDURE dbo.sp_PaymentDensityForEmployee
                                                         @userId int,
                                                         @startingDate datetime2,
                                                         @lastDate datetime2 = null
                                                         AS
                                                         BEGIN
                                                         	SELECT dbo.fn_Payment(1,@userId) PaidAmount,
                                                            dbo.fn_Payment(0,@userId) RejectedAmount,
                                                            dbo.fn_PendingPayment(@userId) PendingAmount,
                                                            @startingDate StartingDate,
                                                            @lastDate LastDate
                                                            FROM Payments p
                                                            WHERE p.CreatedDate BETWEEN @startingDate AND @lastDate
                                                         END
                                                         """;
    }
}
