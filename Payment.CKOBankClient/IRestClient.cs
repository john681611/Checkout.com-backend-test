using Payment.CKOBankClient.Models;

namespace Payment.CKOBankClient;

public interface IRestClient
{
    public Task<CKOPaymentResponse> MakePayment(CKOPaymentRequest paymentRequest);

}
