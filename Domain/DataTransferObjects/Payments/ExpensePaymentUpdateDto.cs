namespace Domain.DataTransferObjects.Payments;

public class PaymentUpdateDto
{
    public string Description { get; set; }
    public bool IsApproved { get; set; }
}