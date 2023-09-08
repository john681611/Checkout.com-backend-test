using System.ComponentModel.DataAnnotations;

namespace Payment.Logic.Models;

public record PaymentDetailsRequest
{
    [Required]
    public Guid PaymentID { get; init; }

    [Required]
    public string MerchantID { get; init; } = "";
}