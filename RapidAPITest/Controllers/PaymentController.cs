using Microsoft.AspNetCore.Mvc;
using Eway.Rapid.Abstractions.Interfaces;
using System.Net.Http;
using Eway.Rapid;
using Eway.Rapid.Abstractions.Request;
using Eway.Rapid.Abstractions.Models;
using RapidAPIProject.Models;
using RapidAPIProject.Services;

namespace RapidAPIProject.Controllers
{
    [ApiController]
    [Route("transaction")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] BasicRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the input request");
                return BadRequest(ModelState);
            }
            try
            {
                var response = await _paymentService.CreateTransaction(request);

                if (response.Errors != null)
                {
                    _logger.LogError("Errors in transaction creation: {Errors}", string.Join(", ", response.Errors));
                    return BadRequest(response);
                }

                _logger.LogInformation("Transaction successfully created.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating transaction.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
