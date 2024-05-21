using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Constants;
using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Core.Views.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExchangeOffice.Application.Handlers.Messages.Rates.CallbackHandlers {
	[CallbackMessageHandler(Callbacks.ReturnedCurrencies)]
	public class ReturnedCurrenciesMessageHandler : ICallbackMessageHandler {
		private readonly ITelegramBotClient _bot;
		private readonly IRateManager _manager;
		private string _cbPrefix;
		public ReturnedCurrenciesMessageHandler(IManagerProvider provider) {
			_bot = provider.GetTelegramBotClient();
			_manager = provider.GetRateManager();
			_cbPrefix = Callbacks.ReturnedCurrencies + "|";
		}
		public async Task ExecuteAsync(Update request) {
			var chatId = request?.CallbackQuery?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}

			var callback = request?.CallbackQuery?.Data;
			if (string.IsNullOrEmpty(callback)) {
				return;
			}

			var splittedCallback = callback.Split('|');
			if (splittedCallback.Length != 2) {
				return;
			}

			var textMessage = await _manager.FillReturnedCurrencyAsync(chatId.ToString(), splittedCallback[1]);
			if (string.IsNullOrEmpty(textMessage)) {
				return;
			}
			var lastMessageId = await _manager.GetLastBotMessageId(chatId.ToString());
			await _bot.DeleteMessageAsync(chatId, messageId: Convert.ToInt32(lastMessageId));
			await _bot.SendTextMessageAsync(chatId, text: textMessage, replyMarkup: MainMenu.Buttons);
		}
	}
}
