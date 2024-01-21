namespace Domain.Entities
{
    public class Expense : BaseEntity
    {
        public decimal Amount { get; set; }
        public string CurrencyType { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Document> Documents { get; set; } = new();
        public Payment Payment { get; set; }
    }
}
