namespace Domain.DataTransferObjects.Documents
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FolderPath { get; set; }
        public int ExpenseId { get; set; }
    }
}
