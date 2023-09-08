using Microsoft.AspNetCore.Mvc;
using Payment.REST.Auth;
using Payment.Logic.Models;
using Payment.Logic.Services;

namespace Payment.REST.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{

    private readonly ILogger<PaymentController> _logger;
    private readonly IPaymentService _paymentService;

    public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService)
    {
        _logger = logger;
        _paymentService = paymentService;
    }

    [HttpPost(Name = "payments")]
    public async Task<ActionResult<PaymentResponse>> MakePayment(PaymentRequest request)
    {
        if(!AuthService.Authenticate(Request.Headers))
            return Unauthorized();
        try
        {
            return await _paymentService.MakePayment(request);;
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500);
        }
    }
}
