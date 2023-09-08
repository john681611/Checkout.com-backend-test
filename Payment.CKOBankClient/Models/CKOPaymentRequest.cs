namespace Payment.CKOBankClient.Models;

public record CKOPaymentRequest
{
    public CKOPaymentRequestCardDetails CardDetails { get; init; } = new();
    public double Ammount { get; init; }
    public string Currency {get; init;} = "";
    public string Referance { get; init;} = "";
}
public record CKOPaymentRequestCardDetails {
    public string Type { get; init; } = "";
    public string Number { get; init; } = "";
    public CKOPaymentRequestExpiry Expiry { get; init; } = new();
    public string CCV { get; init; }
    public string Name { get; init; } = "";
}

public record CKOPaymentRequestExpiry
{
    public int Month {get; init;}
    public int Year {get; init;}
}
