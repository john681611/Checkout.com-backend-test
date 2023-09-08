using Payment.Logic.Models;

namespace Payment.Logic.Services;
public interface IPaymentService
{
    public Task<PaymentResponse> MakePayment(PaymentRequest paymentRequest);
    public Task<PaymentDetailsResponse> GetPaymentRecord(PaymentDetailsRequest paymentDetailsRequest);
}
