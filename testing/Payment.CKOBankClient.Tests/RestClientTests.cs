using Moq;
using Moq.Protected;
using System.Net;
using Newtonsoft.Json;
using Payment.CKOBankClient;
using Payment.CKOBankClient.Models;

namespace Payment.CKOBankClient.Test;
public class RestClientTests: IDisposable
{

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("CKO_BANK_URL", null);
        Environment.SetEnvironmentVariable("CKO_BANK_API_KEY", null);
    }

    [Fact]
    public void Instantiation_NoBankURLArgument_ThrowsArgumentNullException()
    {
        var act = () => new RestClient();

        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'Environment Variable: CKO_BANK_URL')", exception.Message);
    }

    [Fact]
    public void Instantiation_NoBankAPIKeyArgument_ThrowsArgumentNullException()
    {
        Environment.SetEnvironmentVariable("CKO_BANK_URL", "https://Raining.com");
        var act = () => new RestClient();

        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'Environment Variable: CKO_BANK_API_KEY')", exception.Message);
    }

    [Fact]
    public async Task MakePayment_Rejection_ThrowsHttpRequestException()
    {
        SetEnvironmentVariables();
        var mockMessageHandler = new Mock<HttpMessageHandler>();
        mockMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent("")
            });
        var BankClient = new RestClient(new HttpClient(mockMessageHandler.Object));

        var act = async () => await BankClient.MakePayment(new CKOPaymentRequest());
        var exception = await Assert.ThrowsAsync<HttpRequestException>(act);
        Assert.Equal("Unauthorized", exception.Message);
        Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
    }

    [Fact]
    public async Task MakePayment_EmptyResponse_Throws()
    {
        SetEnvironmentVariables();
        var mockMessageHandler = new Mock<HttpMessageHandler>();
        mockMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("")
            });
        var BankClient = new RestClient(new HttpClient(mockMessageHandler.Object));

        var act = async () => await BankClient.MakePayment(new CKOPaymentRequest());
        var exception = await Assert.ThrowsAsync<NullReferenceException>(act);
        Assert.Equal("API call response is empty (https://raining.com/Payment)", exception.Message);
    }


    [Fact]
    public async Task MakePayment_GoodResponse_ReturnsTemperature()
    {
        SetEnvironmentVariables();
        var mockMessageHandler = new Mock<HttpMessageHandler>();
        mockMessageHandler
            .Protected()
            .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new CKOPaymentResponse{
                    StatusCode = 200,
                    StatusText = "All Good"
                }))
            });
        var BankClient = new RestClient(new HttpClient(mockMessageHandler.Object));

        var response = await BankClient.MakePayment(new CKOPaymentRequest());

        Assert.Equal(200, response.StatusCode);
        Assert.Equal("All Good", response.StatusText);
    }


    private void SetEnvironmentVariables()
    {
        Environment.SetEnvironmentVariable("CKO_BANK_URL", "https://Raining.com");
        Environment.SetEnvironmentVariable("CKO_BANK_API_KEY", "SmashHeadOnKeyboard");
    }
}