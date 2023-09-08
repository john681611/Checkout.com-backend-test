using Payment.DB.Models;
using Payment.DB.Services;
using Payment.Logic.Models;
using Payment.CKOBankClient;

namespace Payment.Logic.Services;

public class PaymentService
{
    private PaymentRecordService _paymentRecordService;
    private CKOBankClient _CKOBankClient;

    public PaymentService(PaymentRecordService paymentRecordService, CKOBankClient cKOBankClient)
    {
        _paymentRecordService = paymentRecordService;
        _CKOBankClient = cKOBankClient;
    }
    public async Task<PaymentResponse> MakePayment(PaymentRequest paymentRequest)
    {
        // TODO: VALIDATION
        // var paymentId = await CKOBankClient.MakePayment(payment);
        await _paymentRecordService.CreateAsync(PaymentRequestToRecord(paymentRequest, 200));
        return new PaymentResponse{
            PaymentID = "1234",
            StatusCode = 200
        };
    }
    public async Task<PaymentDetailsResponse> GetPaymentRecord(PaymentDetailsRequest paymentDetailsRequest)
    {
        // TODO: VALIDATION
        var response =  await _paymentRecordService.GetAsync(paymentDetailsRequest.PaymentID, paymentDetailsRequest.MerchantID);
        if (response == null)
            throw new KeyNotFoundException();
        return (PaymentDetailsResponse)response;
    }

    private PaymentRecord PaymentRequestToRecord(PaymentRequest paymentRequest, int resultStatus)
    {
        return  new PaymentRecord{
            PaymentId = "64fb1a3d4691051ac760c57e",
            MerchantID = paymentRequest.MerchantID,
            Ammount = paymentRequest.Ammount,
            Currency = paymentRequest.Currency,
            Referance = paymentRequest.Referance,
            CardType = paymentRequest.CardDetails.Type,
            CardNumberFinalFour = paymentRequest.CardDetails.Number.Substring(paymentRequest.CardDetails.Number.Length-4),
            CardName = paymentRequest.CardDetails.Name,
            ResultStatus = resultStatus
        };
    }
}
