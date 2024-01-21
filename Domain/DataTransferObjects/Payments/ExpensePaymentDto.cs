namespace Domain.DataTransferObjects.Payments
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsApproved { get; set; }
    }
}
