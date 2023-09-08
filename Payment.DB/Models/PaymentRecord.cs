using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Payment.DB.Models;

public class PaymentRecord
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? mongoID { get; init; }
    public Guid PaymentId { get; init; }
    public string MerchantID { get; init; } = "";
    public double Ammount { get; init; }
    public string Currency {get; init;} = "";
    public string Referance { get; init;} = "";
    public string CardNumber { get; init; } = "";
    public string CardName { get; init; } = "";
    public string CardType { get; init; } = "";
    public int ResultStatus { get; init; }
    public string ResultText { get; init; } = "";
}

