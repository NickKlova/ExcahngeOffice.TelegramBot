using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExchangeOffice.Application.Handlers {
	[CallbackMessageHandler("str")]
	public class AcceptedCurrenciesMessageHandler : ICallbackMessageHandler {
		private readonly ITelegramBotClient _bot;
		public AcceptedCurrenciesMessageHandler(IManagerProvider provider) {
			_bot = provider.GetTelegramBotClient();
		}

		public async Task ExecuteAsync(Update request) {
			var chatId = request?.CallbackQuery?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}

			await _bot.SendTextMessageAsync(chatId, text: "clicked");
		}
	}
}
