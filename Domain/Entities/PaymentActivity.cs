namespace Domain.Entities
{
    public class PaymentActivity
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyType { get; set; }
        public int ExpenseId { get; set; }
        public int UserId { get; set; }
        public DateTime PaidDate { get; set; }
    }
}
