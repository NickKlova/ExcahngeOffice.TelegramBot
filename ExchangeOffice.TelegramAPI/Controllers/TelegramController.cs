using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using ExchangeOffice.Core.Views.ReplyMarkups;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;
using static System.Net.Mime.MediaTypeNames;

namespace ExchangeOffice.TelegramAPI.Controllers {
	[Route("api/telegram")]
	[ApiController]
	public class TelegramController : ControllerBase {
		private readonly ITelegramBotClient _client;
		private readonly Dictionary<string, IMessageHandler> _handlers;
		public TelegramController(ITelegramBotClient client, IEnumerable<IMessageHandler> handlers) {
			_client = client;
			_handlers = handlers.ToDictionary(h => h.GetType().GetCustomAttribute<TextMessageHandlerAttribute>().Text);
		}

		[HttpPost("update")]
		public async Task<IActionResult> Update(Update request) {
			await _client.SendTextMessageAsync(request.Message.Chat.Id, "test", replyMarkup: MainMenu.Buttons);
			if (_handlers.TryGetValue(request.Message.Text, out var handler)) {
				handler.Execute(request);
			}
			return Ok();
		}
	}
}
