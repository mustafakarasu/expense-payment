namespace Domain.DataTransferObjects.Payments;

public class PaymentCreationDto
{
    public string Description { get; set; }
    public bool IsApproved { get; set; }
}