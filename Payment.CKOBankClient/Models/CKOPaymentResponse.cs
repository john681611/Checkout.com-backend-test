namespace Payment.CKOBankClient.Models;
public class CKOPaymentResponse
{
    public Guid PaymentID { get; init; }
    public int StatusCode { get; init; }
    public string StatusText { get; init; } = "";
}