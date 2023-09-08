using Payment.CKOBankClient.Models;
using Payment.DB.Models;
using Payment.Logic.Models;

namespace Payment.Logic;

public static class ObjectCasts
{
    public static CKOPaymentRequest PaymentRequestToBankRequest(PaymentRequest paymentRequest)
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

    public static PaymentRecord PaymentRequestToRecord(PaymentRequest paymentRequest, CKOPaymentResponse bankResults)
    {
        return new PaymentRecord
        {
            PaymentId = bankResults.PaymentID,
            MerchantID = paymentRequest.MerchantID,
            Ammount = paymentRequest.Ammount,
            Currency = paymentRequest.Currency,
            Referance = paymentRequest.Referance,
            CardType = paymentRequest.CardDetails.Type,
            CardNumber = new string('X', paymentRequest.CardDetails.Number.Length - 4) + paymentRequest.CardDetails.Number.Substring(paymentRequest.CardDetails.Number.Length - 4),
            CardName = paymentRequest.CardDetails.Name,
            ResultStatus = bankResults.StatusCode,
            ResultText = bankResults.StatusText
        };
    }

    public static PaymentDetailsResponse PaymentRecordToPaymentDetailsResponse(PaymentRecord paymentRecord)
    {
        return new PaymentDetailsResponse
        {
            PaymentId = paymentRecord.PaymentId,
            MerchantID = paymentRecord.MerchantID,
            Ammount = paymentRecord.Ammount,
            Currency = paymentRecord.Currency,
            Referance = paymentRecord.Referance,
            CardNumber = paymentRecord.CardNumber,
            CardName = paymentRecord.CardName,
            CardType = paymentRecord.CardType,
            ResultStatus = paymentRecord.ResultStatus,
            ResultText = paymentRecord.ResultText,
        };
    }
}