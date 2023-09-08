using Payment.DB.Models;

namespace Payment.DB.Services;
public interface IPaymentRecordService
{
    public Task<PaymentRecord?> GetAsync(Guid paymentId, string merchantID);
    public Task CreateAsync(PaymentRecord paymentRecord);
}
