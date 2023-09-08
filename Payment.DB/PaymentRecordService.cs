using Payment.DB.Models;
using MongoDB.Driver;

namespace Payment.DB.Services;

public class PaymentRecordService
{
    private readonly IMongoCollection<PaymentRecord> _PaymentRecordsCollection;

    public PaymentRecordService()
    {
        var mongoClient = new MongoClient(Utils.Utils.GetRequiredEnvironmentVariable("MONGO_CONNECTION"));

        var mongoDatabase = mongoClient.GetDatabase(Utils.Utils.GetRequiredEnvironmentVariable("MONGO_DB"));

        _PaymentRecordsCollection = mongoDatabase.GetCollection<PaymentRecord>("PaymentRecords");
    }

    public async Task<PaymentRecord?> GetAsync(string paymentId, string merchantID) =>
        await _PaymentRecordsCollection.Find(x => x.PaymentId == paymentId && x.MerchantID == merchantID).FirstOrDefaultAsync();

    public async Task CreateAsync(PaymentRecord paymentRecord) =>
        await _PaymentRecordsCollection.InsertOneAsync(paymentRecord);

}