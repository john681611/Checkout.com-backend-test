namespace Payment.Logic.Tests;

using Payment.DB.Services;
using Payment.Logic.Models;
using Payment.CKOBankClient.Models;
using Payment.CKOBankClient;
using Payment.Logic.Services;
using Moq;
using Payment.DB.Models;

public class PaymentServiceTests
{
    [Fact]
    public async Task MakePayment_BankClientBreaks_ThrowsException()
    {
        var mockBankClient = new Mock<IRestClient>();
        var mockMongoClient = new Mock<IPaymentRecordService>();

        mockBankClient.Setup(x => x.MakePayment(It.IsAny<CKOPaymentRequest>())).ThrowsAsync(new Exception("BOOM"));

        var service = new PaymentService(mockMongoClient.Object, mockBankClient.Object);

        var act = () => service.MakePayment(new PaymentRequest());
        var exception = await Assert.ThrowsAsync<Exception>(act);
        Assert.Equal("BOOM", exception.Message);
    }

    [Fact]
    public async Task MakePayment_MongoClientBreaks_ThrowsException()
    {
        var mockBankClient = new Mock<IRestClient>();
        var mockMongoClient = new Mock<IPaymentRecordService>();

        mockBankClient.Setup(x => x.MakePayment(It.IsAny<CKOPaymentRequest>())).ReturnsAsync(new CKOPaymentResponse
        {
            PaymentID = Guid.NewGuid(),
            StatusCode = 200,
            StatusText = "Accepted"
        });
        mockMongoClient.Setup(x => x.CreateAsync(It.IsAny<PaymentRecord>())).ThrowsAsync(new Exception("BOOM"));

        var service = new PaymentService(mockMongoClient.Object, mockBankClient.Object);

        var act = () => service.MakePayment(new PaymentRequest
        {
            CardDetails = new PaymentRequestCardDetails
            {
                Number = "1234"
            }
        });
        var exception = await Assert.ThrowsAsync<Exception>(act);
        Assert.Equal("BOOM", exception.Message);
    }

    [Fact]
    public async void MakePayment_AllGood_ReturnsPaymentResponse()
    {
        var mockBankClient = new Mock<IRestClient>();
        var mockMongoClient = new Mock<IPaymentRecordService>();

        mockBankClient.Setup(x => x.MakePayment(It.IsAny<CKOPaymentRequest>())).ReturnsAsync(new CKOPaymentResponse
        {
            PaymentID = Guid.NewGuid(),
            StatusCode = 200,
            StatusText = "Accepted"
        });

        var service = new PaymentService(mockMongoClient.Object, mockBankClient.Object);

        var result = await service.MakePayment(new PaymentRequest
        {
            CardDetails = new PaymentRequestCardDetails
            {
                Number = "1234"
            }
        });

        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Accepted", result.StatusText);
    }

    [Fact]
    public async Task GetPaymentRecord_MongoClientBreaks_ThrowsException()
    {
        var mockBankClient = new Mock<IRestClient>();
        var mockMongoClient = new Mock<IPaymentRecordService>();

        mockMongoClient.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>())).ThrowsAsync(new Exception("BOOM"));

        var service = new PaymentService(mockMongoClient.Object, mockBankClient.Object);

        var act = () => service.GetPaymentRecord(new PaymentDetailsRequest());
        var exception = await Assert.ThrowsAsync<Exception>(act);
        Assert.Equal("BOOM", exception.Message);
    }

    [Fact]
    public async Task GetPaymentRecord_MongoClientNullResponse_ThrowsKeyNotFoundException()
    {
        var mockBankClient = new Mock<IRestClient>();
        var mockMongoClient = new Mock<IPaymentRecordService>();

        mockMongoClient.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>()));

        var service = new PaymentService(mockMongoClient.Object, mockBankClient.Object);

        var act = () => service.GetPaymentRecord(new PaymentDetailsRequest());
        await Assert.ThrowsAsync<KeyNotFoundException>(act);
    }

    [Fact]
    public async Task GetPaymentRecord_AllGood_ReturnsPaymentRecord()
    {
        var mockBankClient = new Mock<IRestClient>();
        var mockMongoClient = new Mock<IPaymentRecordService>();

        mockMongoClient.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(new PaymentRecord{
            MerchantID = "1234"
        });

        var service = new PaymentService(mockMongoClient.Object, mockBankClient.Object);

        var response =  await service.GetPaymentRecord(new PaymentDetailsRequest());
        
        Assert.Equal("1234", response.MerchantID);
    }
}