using Payment.DB.Services;
using Payment.Logic.Models;
using Payment.CKOBankClient;

namespace Payment.Logic.Services;

public class PaymentService: IPaymentService
{
    private readonly IPaymentRecordService _paymentRecordService;
    private readonly IRestClient _CKOBankClient;

    public PaymentService(PaymentRecordService paymentRecordService, RestClient cKOBankClient)
    {
        _paymentRecordService = paymentRecordService;
        _CKOBankClient = cKOBankClient;
    }
    public async Task<PaymentResponse> MakePayment(PaymentRequest paymentRequest)
    {
        var bankResponse = await _CKOBankClient.MakePayment(ObjectCasts.PaymentRequestToBankRequest(paymentRequest));
        await _paymentRecordService.CreateAsync(ObjectCasts.PaymentRequestToRecord(paymentRequest, bankResponse));
        return new PaymentResponse
        {
            PaymentID = bankResponse.PaymentID,
            StatusCode = bankResponse.StatusCode,
            StatusText = bankResponse.StatusText
        };
    }
    public async Task<PaymentDetailsResponse> GetPaymentRecord(PaymentDetailsRequest paymentDetailsRequest)
    {
        var response = await _paymentRecordService.GetAsync(paymentDetailsRequest.PaymentID, paymentDetailsRequest.MerchantID);
        if (response == null)
            throw new KeyNotFoundException();
        return ObjectCasts.PaymentRecordToPaymentDetailsResponse(response);
    }
}
