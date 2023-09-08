namespace Payment.Logic.Models;

public class PaymentResponse
{
    public Guid PaymentID { get; init; }
    public int StatusCode { get; init; }
    public string StatusText { get; init; } = "";
}
