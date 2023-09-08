using Payment.DB.Models;
using Payment.DB.Services;
using Payment.Logic.Models;
using Payment.CKOBankClient;
using Payment.CKOBankClient.Models;

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
        var bankResponse = await _CKOBankClient.MakePayment(PaymentRequestToBankRequest(paymentRequest));
        await _paymentRecordService.CreateAsync(PaymentRequestToRecord(paymentRequest, bankResponse));
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
        return PaymentRecordToPaymentDetailsResponse(response);
    }

    private CKOPaymentRequest PaymentRequestToBankRequest(PaymentRequest paymentRequest)
    {
        return new CKOPaymentRequest
        {
            Ammount = paymentRequest.Ammount,
            Currency = paymentRequest.Currency,
            Referance = paymentRequest.Referance,
            CardDetails = new CKOPaymentRequestCardDetails
            {
                Type = paymentRequest.CardDetails.Type,
                Number = paymentRequest.CardDetails.Number,
                CCV = paymentRequest.CardDetails.CCV,
                Name = paymentRequest.CardDetails.Name,
                Expiry = new CKOPaymentRequestExpiry
                {
                    Month = paymentRequest.CardDetails.Expiry.Month,
                    Year = paymentRequest.CardDetails.Expiry.Year,
                },
            }
        };
    }

    private PaymentRecord PaymentRequestToRecord(PaymentRequest paymentRequest, CKOPaymentResponse bankResults)
    {
        return new PaymentRecord
        {
            PaymentId = bankResults.PaymentID,
            MerchantID = paymentRequest.MerchantID,
            Ammount = paymentRequest.Ammount,
            Currency = paymentRequest.Currency,
            Referance = paymentRequest.Referance,
            CardType = paymentRequest.CardDetails.Type,
            CardNumberFinalFour = paymentRequest.CardDetails.Number.Substring(paymentRequest.CardDetails.Number.Length - 4),
            CardName = paymentRequest.CardDetails.Name,
            ResultStatus = bankResults.StatusCode,
            ResultText = bankResults.StatusText
        };
    }

    private PaymentDetailsResponse PaymentRecordToPaymentDetailsResponse(PaymentRecord paymentRecord)
    {
        return new PaymentDetailsResponse
        {
            PaymentId = paymentRecord.PaymentId,
            MerchantID = paymentRecord.MerchantID,
            Ammount = paymentRecord.Ammount,
            Currency = paymentRecord.Currency,
            Referance = paymentRecord.Referance,
            CardNumberFinalFour = paymentRecord.CardNumberFinalFour,
            CardName = paymentRecord.CardName,
            CardType = paymentRecord.CardType,
            ResultStatus = paymentRecord.ResultStatus,
            ResultText = paymentRecord.ResultText,
        };
    }
}
