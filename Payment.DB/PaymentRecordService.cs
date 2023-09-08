using Payment.DB.Models;
using MongoDB.Driver;

namespace Payment.DB.Services;

public class PaymentRecordService: IPaymentRecordService
{
    private readonly IMongoCollection<PaymentRecord> _PaymentRecordsCollection;

    public PaymentRecordService(IMongoClient? client = null)
    {
        var mongoClient = client ?? new MongoClient(Utils.EnvUtils.GetRequiredEnvironmentVariable("MONGO_CONNECTION"));

        var mongoDatabase = mongoClient.GetDatabase(Utils.EnvUtils.GetRequiredEnvironmentVariable("MONGO_DB"));

        _PaymentRecordsCollection = mongoDatabase.GetCollection<PaymentRecord>("PaymentRecords");
    }

    public async Task<PaymentRecord?> GetAsync(Guid paymentId, string merchantID)
    {
        return await  _PaymentRecordsCollection.Find(x => x.PaymentId == paymentId && x.MerchantID == merchantID).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(PaymentRecord paymentRecord) =>
        await _PaymentRecordsCollection.InsertOneAsync(paymentRecord);

}