using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Constants;
using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using ExchangeOffice.Core.Views.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExchangeOffice.Application.Handlers.Messages {
	[TextMessageHandler(MenuTitles.Back)]
	public class BackMessageHandler : ITextMessageHandler {
		private readonly ITelegramBotClient _bot;
		public BackMessageHandler(IManagerProvider provider) {
			_bot = provider.GetTelegramBotClient();
		}

		public async Task ExecuteAsync(Update request) {
			var chatId = request?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}

			await _bot.SendTextMessageAsync(chatId, "test", replyMarkup: MainMenu.Buttons);
		}
	}
}
