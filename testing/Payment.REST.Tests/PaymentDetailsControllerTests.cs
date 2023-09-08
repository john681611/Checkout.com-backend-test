using Moq;
using Payment.REST.Controllers;
using Payment.Logic.Models;
using Microsoft.Extensions.Logging;
using Payment.Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Payment.REST.Tests;

public class PaymentDetailsControllerTests
{

    [Fact]
    public async Task GetPaymentDetails_Unauthentiated_ReturnsUnauthenticated()
    {
        var requestMock = new Mock<HttpRequest>();
        requestMock.Setup(x => x.Scheme).Returns("http");
        requestMock.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:8080"));
        requestMock.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api"));
        requestMock.Setup(x => x.Headers).Returns(new HeaderDictionary());

        var httpContext = Mock.Of<HttpContext>(_ =>
            _.Request == requestMock.Object
        );

        //Controller needs a controller context 
        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext,
        };

        var mockLogger = new Mock<ILogger<PaymentDetailsController>>();
        var mockService = new Mock<IPaymentService>();
        var paymentDetailsController = new PaymentDetailsController(mockLogger.Object, mockService.Object)
        {
            ControllerContext = controllerContext
        };


        var reponse = await paymentDetailsController.GetPaymentDetails(new PaymentDetailsRequest());
        var result = reponse.Result as UnauthorizedResult;

        Assert.Equal(401, result?.StatusCode);
    }

    [Fact]
    public async Task GetPaymentDetails_ServiceError_Returns500()
    {
        var requestMock = new Mock<HttpRequest>();
        requestMock.Setup(x => x.Scheme).Returns("http");
        requestMock.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:8080"));
        requestMock.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api"));
        requestMock.Setup(x => x.Headers).Returns(new HeaderDictionary{
            {"Authorization", "Bearer 1234"}
        });

        var httpContext = Mock.Of<HttpContext>(_ =>
            _.Request == requestMock.Object
        );

        //Controller needs a controller context 
        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext,
        };

        var mockLogger = new Mock<ILogger<PaymentDetailsController>>();
        var mockService = new Mock<IPaymentService>();
        mockService.Setup(x => x.GetPaymentRecord(It.IsAny<PaymentDetailsRequest>())).ThrowsAsync(new Exception("Boom"));

        var paymentDetailsController = new PaymentDetailsController(mockLogger.Object, mockService.Object)
        {
            ControllerContext = controllerContext
        };


        var reponse = await paymentDetailsController.GetPaymentDetails(new PaymentDetailsRequest());
        var result = reponse.Result as StatusCodeResult;

        Assert.Equal(500, result?.StatusCode);
    }

    [Fact]
    public async Task GetPaymentDetails_NotFound_ReturnsNotFound()
    {
        var requestMock = new Mock<HttpRequest>();
        requestMock.Setup(x => x.Scheme).Returns("http");
        requestMock.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:8080"));
        requestMock.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api"));
        requestMock.Setup(x => x.Headers).Returns(new HeaderDictionary{
            {"Authorization", "Bearer 1234"}
        });

        var httpContext = Mock.Of<HttpContext>(_ =>
            _.Request == requestMock.Object
        );

        //Controller needs a controller context 
        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext,
        };

        var mockLogger = new Mock<ILogger<PaymentDetailsController>>();
        var mockService = new Mock<IPaymentService>();
        mockService.Setup(x => x.GetPaymentRecord(It.IsAny<PaymentDetailsRequest>())).ThrowsAsync(new KeyNotFoundException());

        var paymentDetailsController = new PaymentDetailsController(mockLogger.Object, mockService.Object)
        {
            ControllerContext = controllerContext
        };


        var reponse = await paymentDetailsController.GetPaymentDetails(new PaymentDetailsRequest());
        var result = reponse.Result as NotFoundResult;

        Assert.Equal(404, result?.StatusCode);
    }

    [Fact]
    public async Task GetPaymentDetails_AllGood_ReturnsPaymentRecord()
    {
        var requestMock = new Mock<HttpRequest>();
        requestMock.Setup(x => x.Scheme).Returns("http");
        requestMock.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:8080"));
        requestMock.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api"));
        requestMock.Setup(x => x.Headers).Returns(new HeaderDictionary{
            {"Authorization", "Bearer 1234"}
        });

        var httpContext = Mock.Of<HttpContext>(_ =>
            _.Request == requestMock.Object
        );

        //Controller needs a controller context 
        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext,
        };

        var mockLogger = new Mock<ILogger<PaymentDetailsController>>();
        var mockService = new Mock<IPaymentService>();
        mockService.Setup(x => x.GetPaymentRecord(It.IsAny<PaymentDetailsRequest>())).ReturnsAsync(new PaymentDetailsResponse
        {
            MerchantID = "1234"
        });

        var paymentDetailsController = new PaymentDetailsController(mockLogger.Object, mockService.Object)
        {
            ControllerContext = controllerContext
        };


        var reponse = await paymentDetailsController.GetPaymentDetails(new PaymentDetailsRequest());

        Assert.Equal("1234", reponse?.Value?.MerchantID);
    }
}