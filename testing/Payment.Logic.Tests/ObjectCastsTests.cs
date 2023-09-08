namespace Payment.Logic.Tests;

using Payment.DB.Services;
using Payment.Logic.Models;
using Payment.CKOBankClient.Models;
using Payment.CKOBankClient;
using Payment.Logic.Services;
using Moq;
using Payment.DB.Models;

public class ObjectCastsTests
{
    [Fact]
    public void PaymentRequestToBankRequest()
    {
        var result = ObjectCasts.PaymentRequestToBankRequest(new PaymentRequest
        {
            MerchantID = "MerchantID",
            Ammount = 123,
            Currency = "USD",
            Referance = "Death Star Money",
            CardDetails = new PaymentRequestCardDetails
            {
                Type = "EmpireExpress",
                Number = "1234567890",
                CCV = "123",
                Name = "Ld Vador",
                Expiry = new PaymentRequestExpiry
                {
                    Month = 1,
                    Year = 1,
                },
            }
        });

        Assert.Equal(new CKOPaymentRequest
        {
            Ammount = 123,
            Currency = "USD",
            Referance = "Death Star Money",
            CardDetails = new CKOPaymentRequestCardDetails
            {
                Type = "EmpireExpress",
                Number = "1234567890",
                CCV = "123",
                Name = "Ld Vador",
                Expiry = new CKOPaymentRequestExpiry
                {
                    Month = 1,
                    Year = 1,
                },
            }
        }, result);
    }

    [Fact]
    public void PaymentRequestToRecord()
    {
        var paymentRequest = new PaymentRequest
        {
            MerchantID = "MerchantID",
            Ammount = 123,
            Currency = "USD",
            Referance = "Death Star Money",
            CardDetails = new PaymentRequestCardDetails
            {
                Type = "EmpireExpress",
                Number = "1234567890",
                CCV = "123",
                Name = "Ld Vador",
                Expiry = new PaymentRequestExpiry
                {
                    Month = 1,
                    Year = 1,
                },
            }
        };

        var bankResults = new CKOPaymentResponse{
            PaymentID = Guid.NewGuid(),
            StatusCode = 200,
            StatusText = "Accepted"
        };

        var expected = new PaymentRecord
        {
            PaymentId = bankResults.PaymentID,
            MerchantID = "MerchantID",
            Ammount = 123,
            Currency = "USD",
            Referance = "Death Star Money",
            CardType = "EmpireExpress",
            CardNumber = "XXXXXX7890",
            CardName = "Ld Vador",
            ResultStatus = 200,
            ResultText = "Accepted"
        };

        var result = ObjectCasts.PaymentRequestToRecord(paymentRequest, bankResults);

        

        Assert.Equal(expected.PaymentId, result.PaymentId);
        Assert.Equal(expected.MerchantID, result.MerchantID);
        Assert.Equal(expected.Ammount, result.Ammount);
        Assert.Equal(expected.Currency, result.Currency);
        Assert.Equal(expected.Referance, result.Referance);
        Assert.Equal(expected.CardType, result.CardType);
        Assert.Equal(expected.CardNumber, result.CardNumber);
        Assert.Equal(expected.CardName, result.CardName);
        Assert.Equal(expected.ResultStatus, result.ResultStatus);
        Assert.Equal(expected.ResultText, result.ResultText);
    }

    [Fact]
    public void PaymentRecordToPaymentDetailsResponse()
    {
        var paymentRecord = new PaymentRecord
        {
            PaymentId = Guid.NewGuid(),
            MerchantID = "MerchantID",
            Ammount = 123,
            Currency = "USD",
            Referance = "Death Star Money",
            CardType = "EmpireExpress",
            CardNumber = "XXXXXX7890",
            CardName = "Ld Vador",
            ResultStatus = 200,
            ResultText = "Accepted"
        };

        var expected = new PaymentDetailsResponse{
            PaymentId = paymentRecord.PaymentId,
            MerchantID = "MerchantID",
            Ammount = 123,
            Currency = "USD",
            Referance = "Death Star Money",
            CardType = "EmpireExpress",
            CardNumber = "XXXXXX7890",
            CardName = "Ld Vador",
            ResultStatus = 200,
            ResultText = "Accepted"
        };

        var result = ObjectCasts.PaymentRecordToPaymentDetailsResponse(paymentRecord);

        Assert.Equal(expected.PaymentId, result.PaymentId);
        Assert.Equal(expected.MerchantID, result.MerchantID);
        Assert.Equal(expected.Ammount, result.Ammount);
        Assert.Equal(expected.Currency, result.Currency);
        Assert.Equal(expected.Referance, result.Referance);
        Assert.Equal(expected.CardType, result.CardType);
        Assert.Equal(expected.CardNumber, result.CardNumber);
        Assert.Equal(expected.CardName, result.CardName);
        Assert.Equal(expected.ResultStatus, result.ResultStatus);
        Assert.Equal(expected.ResultText, result.ResultText);

    }
}