using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Telegram.Bot.Types;

namespace ExchangeOffice.TelegramAPI.Controllers {
	[Route("api/telegram")]
	[ApiController]
	public class TelegramController : ControllerBase {
		//private readonly Dictionary<string, IMessageHandler> _handlers;
		//public TelegramController(IEnumerable<IMessageHandler> handlers) {
		//	_handlers = handlers
		//		.Select(h => new { Handler = h, Attribute = h.GetType().GetCustomAttribute<TextMessageHandlerAttribute>() })
		//		.Where(x => x.Attribute != null)
		//		.ToDictionary(x => x.Attribute!.Text, x => x.Handler);
		//}

		[HttpPost("update")]
		public async Task<IActionResult> Update(Update request) {
			//if (request == null) {
			//	return Ok();
			//}

			//var text = request?.Message?.Text;
			//if (string.IsNullOrEmpty(text)) {
			//	return Ok();
			//}

			//if (_handlers.TryGetValue(text, out var handler)) {
			//	await handler.ExecuteAsync(request!);
			//}
			return Ok();
		}
	}
}
