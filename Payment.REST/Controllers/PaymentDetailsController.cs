using Microsoft.AspNetCore.Mvc;
using Payment.REST.Auth;
using Payment.Logic.Models;
using Payment.Logic.Services;

namespace Payment.REST.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentDetailsController : ControllerBase
{

    private readonly ILogger<PaymentDetailsController> _logger;
    private readonly IPaymentService _paymentService;

    public PaymentDetailsController(ILogger<PaymentDetailsController> logger, IPaymentService paymentService)
    {
        _logger = logger;
        _paymentService = paymentService;
    }

    [HttpPost(Name = "paymentDetails")]
    public async Task<ActionResult<PaymentDetailsResponse>> GetPaymentDetails(PaymentDetailsRequest request)
    {
        if (!AuthService.Authenticate(Request.Headers))
            return Unauthorized();
        try
        {
            return await _paymentService.GetPaymentRecord(request);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500);
        }
    }
}
