using Payment.DB.Services;
using Payment.Logic.Models;
using Payment.CKOBankClient;

namespace Payment.Logic.Services;

public class PaymentService
{
    private readonly PaymentRecordService _paymentRecordService;
    private readonly RestClient _CKOBankClient;

    public PaymentService(PaymentRecordService paymentRecordService, RestClient cKOBankClient)
    {
        _paymentRecordService = paymentRecordService;
        _CKOBankClient = cKOBankClient;
    }
    public async Task<PaymentResponse> MakePayment(PaymentRequest paymentRequest)
    {
        // TODO: VALIDATION
        var bankResponse = await _CKOBankClient.MakePayment(ObjectCasts.PaymentRequestToBankRequest(paymentRequest));
        await _paymentRecordService.CreateAsync(ObjectCasts.PaymentRequestToRecord(paymentRequest, bankResponse));
        return new PaymentResponse
        {
            PaymentID = "1234",
            StatusCode = 200
        };
    }
    public async Task<PaymentDetailsResponse> GetPaymentRecord(PaymentDetailsRequest paymentDetailsRequest)
    {
        // TODO: VALIDATION
        var response = await _paymentRecordService.GetAsync(paymentDetailsRequest.PaymentID, paymentDetailsRequest.MerchantID);
        if (response == null)
            throw new KeyNotFoundException();
        return ObjectCasts.PaymentRecordToPaymentDetailsResponse(response);
    }
}
