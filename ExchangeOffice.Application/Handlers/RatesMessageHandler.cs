using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Constants;
using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using ExchangeOffice.Application.Views.InlineMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExchangeOffice.Application.Handlers {
	[TextMessageHandler(MenuTitles.Rates)]
	public class RatesMessageHandler : ITextMessageHandler {
		private ITelegramBotClient _bot;
		public RatesMessageHandler(IManagerProvider provider) {
			_bot = provider.GetTelegramBotClient();
		}

		public async Task ExecuteAsync(Update request) {
			var chatId = request?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}

			await _bot.SendTextMessageAsync(chatId, text: "test", replyMarkup: RatesMenu.Buttons);
		}
	}
}
