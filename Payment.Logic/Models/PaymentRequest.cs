using System.ComponentModel.DataAnnotations;

namespace Payment.Logic.Models;

public record PaymentRequest
{
    [Required]
    public PaymentRequestCardDetails CardDetails { get; init; } = new();

    [Required]
    public double Ammount { get; init; }

    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string Currency {get; init;} = "";

    [Required]
    public string Referance { get; init;} = "";

    [Required]
    public string MerchantID { get; init; } = "";
}
public record PaymentRequestCardDetails {

    [Required]
    public string Type { get; init; } = "";

    [Required]
    [RegularExpression(@"\b4[0-9]{12}(?:[0-9]{3})?\b", ErrorMessage = "Invalid Card Number.")]
    public string Number { get; init; } = "";

    [Required]
    public PaymentRequestExpiry Expiry { get; init; } = new();

    [Required]
    [RegularExpression(@"[0-9]{3}", ErrorMessage = "Invalid CVV.")]
    public string CCV { get; init; } = "";

    [Required]
    public string Name { get; init; } = "";

}

public record PaymentRequestExpiry
{

    [Required]
    [Range(1,12)]
    public int Month {get; init;}
    
    [Required]
    [Range(1970,2200)] //Maybe not our responability if the exipry out out :D Bank should Reject
    public int Year {get; init;}
}
