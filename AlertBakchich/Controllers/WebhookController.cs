using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using AlertBakchich.Models;
using AlertBakchich.Hubs;

namespace AlertBakchich.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly IHubContext<PaymentHub> _hubContext;

        public WebhookController(IHubContext<PaymentHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Payment payment)
        {
            if (payment == null)
                return BadRequest();

            // Broadcast to all connected clients
            await _hubContext.Clients.All.SendAsync("ReceivePayment", payment);
            return Ok();
        }
    }
}
