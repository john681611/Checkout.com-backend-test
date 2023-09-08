using CKO.MockBank.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Payment.CKOBankClient.Models;

namespace CKO.MockBank.API.Controllers;

[ApiController]
[Route("[controller]")]
public class Payment : ControllerBase
{

    private readonly ILogger<Payment> _logger;

    public Payment(ILogger<Payment> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "payment")]
    public ActionResult<CKOPaymentResponse> Post(CKOPaymentRequest request)
    {
        Guid myuuid = Guid.NewGuid();
        string PaymentId = myuuid.ToString();
        if(!AuthService.Authenticate(Request.Headers))
            return Unauthorized();
        if (request.Referance == "NO MONEY")
            return new CKOPaymentResponse
            {
                PaymentID = PaymentId,
                StatusCode = 402,
                StatusText = "INSUFFICENT FUNDS"
            };
        if (request.Referance == "REJECT ME")
            return new CKOPaymentResponse
            {
                PaymentID = PaymentId,
                StatusCode = 401,
                StatusText = "REJECTED"
            };
        return new CKOPaymentResponse
        {
            PaymentID = PaymentId,
            StatusCode = 200,
            StatusText = "Approved"
        };
    }
}
