namespace Domain.Entities
{
    public class Document : BaseEntity
    {
        public string Name { get; set; }
        public string FolderPath { get; set; }

        public int ExpenseId { get; set; }
        public Expense Expense { get; set; }
    }
}
