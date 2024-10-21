using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Magdy.APIs.Errors;
using Store.Magdy.Core.Services.Contract;
using Stripe;

namespace Store.Magdy.APIs.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("{basketId}")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentIntent(string basketId)
        {
            if (basketId is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var basket = await _paymentService.CreateOrUpdatePaymentIntentIdAsync(basketId);

            if (basket is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(basket);
        }

        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        const string endpointSecret = "whsec_72f586ad06fc4df592ee3d30922c41e606fd7a4e5664d0c6ab40f61d09cf2f23";
        [HttpPost("webhook")] //https://localhost:7141/api/payments/webhook
        public async Task<IActionResult> Index()
            {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                try
                {
                    var stripeEvent = EventUtility.ConstructEvent(json,
                        Request.Headers["Stripe-Signature"], endpointSecret, throwOnApiVersionMismatch:false);

                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                    // Handle the event
                    if (stripeEvent.Type == "payment_intent.payment_failed")
                    {
                        await _paymentService.UpdatePaymentIntentForSucceedOrFailed(paymentIntent.Id, false);
                    }
                    else if (stripeEvent.Type == "payment_intent.succeeded")
                    {
                        await _paymentService.UpdatePaymentIntentForSucceedOrFailed(paymentIntent.Id, true);
                    }
                    // ... handle other event types
                    else
                    {
                        Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    }

                    return Ok();
                }
                catch (StripeException e)
                {
                    return BadRequest();
                }
            }
        }
}
