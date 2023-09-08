using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Payment.Logic.Models;
using Payment.Logic.Services;
using Payment.REST.Controllers;

namespace Payment.REST.Tests;

public class PaymentControllerTests
{
    [Fact]
    public async Task MakePayment_Unauthentiated_ReturnsUnauthenticated()
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

        var mockLogger = new Mock<ILogger<PaymentController>>();
        var mockService = new Mock<IPaymentService>();
        var PaymentController = new PaymentController(mockLogger.Object, mockService.Object)
        {
            ControllerContext = controllerContext
        };


        var reponse = await PaymentController.MakePayment(new PaymentRequest());
        var result = reponse.Result as UnauthorizedResult;

        Assert.Equal(401, result?.StatusCode);
    }

    [Fact]
    public async Task MakePayment_ServiceError_Returns500()
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

        var mockLogger = new Mock<ILogger<PaymentController>>();
        var mockService = new Mock<IPaymentService>();
        mockService.Setup(x => x.MakePayment(It.IsAny<PaymentRequest>())).ThrowsAsync(new Exception("Boom"));

        var PaymentController = new PaymentController(mockLogger.Object, mockService.Object)
        {
            ControllerContext = controllerContext
        };


        var reponse = await PaymentController.MakePayment(new PaymentRequest());
        var result = reponse.Result as StatusCodeResult;

        Assert.Equal(500, result?.StatusCode);
    }

    [Fact]
    public async Task MakePayment_AllGood_ReturnsPaymentRecord()
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

        var mockLogger = new Mock<ILogger<PaymentController>>();
        var mockService = new Mock<IPaymentService>();
        mockService.Setup(x => x.MakePayment(It.IsAny<PaymentRequest>())).ReturnsAsync(new PaymentResponse
        {
            StatusText = "Accepted",
            StatusCode = 200
        });

        var PaymentController = new PaymentController(mockLogger.Object, mockService.Object)
        {
            ControllerContext = controllerContext
        };


        var reponse = await PaymentController.MakePayment(new PaymentRequest());

        Assert.Equal("Accepted", reponse?.Value?.StatusText);
        Assert.Equal(200, reponse?.Value?.StatusCode);
    }
}