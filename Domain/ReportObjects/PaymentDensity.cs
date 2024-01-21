namespace Domain.ReportObjects
{
    public class PaymentDensity
    {
        public decimal PaidAmount { get; set; }
        public decimal RejectedAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime LastDate { get; set; }
    }
}
