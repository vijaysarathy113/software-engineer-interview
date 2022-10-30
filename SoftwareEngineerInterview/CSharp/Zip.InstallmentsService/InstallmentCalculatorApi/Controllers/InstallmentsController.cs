using InstallmentCalculatorApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zip.InstallmentsService;

namespace InstallmentCalculatorApi.Controllers
{
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class InstallmentsController : ControllerBase
    {
        private readonly ILogger<InstallmentsController> _logger;
        private readonly PaymentPlanFactory _paymentPlanFactory;
        public InstallmentsController(ILogger<InstallmentsController> _logger, PaymentPlanFactory paymentPlanFactory)
        {
            this._logger = _logger;
            _paymentPlanFactory = paymentPlanFactory;
        }


        [HttpGet("{purchaseAmount}")]
        public IActionResult Get(decimal purchaseAmount)
        {
            try
            {
                if (purchaseAmount <= 0)
                    return BadRequest($"{nameof(purchaseAmount)} {purchaseAmount} is invalid");
                PaymentPlan PaymentPlan = _paymentPlanFactory.CreatePaymentPlan(purchaseAmount);
                if (PaymentPlan.Id == Guid.Empty)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                var installment = PaymentPlan.Installments.Select(i => new InstallmentResponse { PurchaseAmount = i.Amount, DueDate = i.DueDate });
                return Ok(installment);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(InstallmentsController)} {nameof(Get)} {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}
