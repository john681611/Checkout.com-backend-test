namespace Payment.Logic.Models;

public record PaymentDetailsRequest
{
    public string PaymentID { get; init; } = "";
    public string MerchantID { get; init; } = "";
}