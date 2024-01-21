namespace Domain.Entities
{
    public class Payment : BaseEntity
    {
        public string Description { get; set; }
        public bool IsApproved { get; set; }
        public int ExpenseId { get; set; }
        public Expense Expense { get; set; }

    }
}
