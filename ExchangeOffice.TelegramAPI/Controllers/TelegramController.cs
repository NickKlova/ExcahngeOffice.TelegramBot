using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace ExchangeOffice.TelegramAPI.Controllers {
	[Route("api/telegram")]
	[ApiController]
	public class TelegramController : ControllerBase {
		[HttpPost("update")]
		public async Task<IActionResult> Update(Update request) {
			return Ok();
		}
	}
}
