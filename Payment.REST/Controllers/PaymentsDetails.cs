using Microsoft.AspNetCore.Mvc;
using Payment.REST.Auth;
using Payment.Logic.Models;

namespace Payment.REST.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentDetailsController : ControllerBase
{

    private readonly ILogger<PaymentDetailsController> _logger;

    public PaymentDetailsController(ILogger<PaymentDetailsController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "paymentDetails")]
    public ActionResult<PaymentDetailsResponse> Payments(PaymentDetailsRequest request)
    { 
         if(!AuthService.Authenticate(Request.Headers))
            return Unauthorized();
        return new PaymentDetailsResponse{};
    }
}
