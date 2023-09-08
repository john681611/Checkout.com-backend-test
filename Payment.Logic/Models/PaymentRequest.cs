namespace Payment.Logic.Models;

public record PaymentRequest
{
    public PaymentRequestCardDetails CardDetails { get; init; } = new();
    public double Ammount { get; init; }
    public string Currency {get; init;} = "";
    public string Referance { get; init;} = "";
    public string MerchantID { get; init; } = "";
}
public record PaymentRequestCardDetails {
    public string Type { get; init; } = "";
    public string Number { get; init; } = "";
    public PaymentRequestExpiry Expiry { get; init; } = new();
    public int CCV { get; init; }
    public string Name { get; init; } = "";

}

public record PaymentRequestExpiry
{
    public int Month {get; init;}
    public int Year {get; init;}
}
