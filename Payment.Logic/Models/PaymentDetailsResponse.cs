
namespace Payment.Logic.Models;

public class PaymentDetailsResponse
{
    public Guid PaymentId { get; set; }
    public string MerchantID { get; set; } = "";
    public double Ammount { get; init; }
    public string Currency {get; init;} = "";
    public string Referance { get; init;} = "";
    public string CardNumberFinalFour { get; init; } = "";
    public string CardName { get; init; } = "";
    public string CardType { get; init; } = "";
    public int ResultStatus { get; init; }
    public string ResultText { get; init; } = "";
}
