using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Constants;
using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Application.Views.InlineMarkups;
using ExchangeOffice.Common.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangeOffice.Application.Handlers.Messages.Rates.CallbackHandlers {
    [CallbackMessageHandler(Callbacks.AcceptedCurrencies)]
    public class AcceptedCurrenciesMessageHandler : ICallbackMessageHandler {
        private readonly ITelegramBotClient _bot;
        private readonly IRateManager _manager;
		private string _cbPrefix;
		public AcceptedCurrenciesMessageHandler(IManagerProvider provider) {
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

            await _manager.FillAcceptedCurrencyAsync(chatId.ToString(), splittedCallback[1]);
            var returnedCurrencies = await _manager.GetReceivedCurrenciesAsync(chatId.ToString(), splittedCallback[1]);
            var keyboard = GetReturnedCurrenciesKeyboard(returnedCurrencies);

            var lastMessageId = await _manager.GetLastBotMessageId(chatId.ToString());
            await _bot.DeleteMessageAsync(chatId, messageId: Convert.ToInt32(lastMessageId));
			var msg = await _bot.SendTextMessageAsync(chatId, text: RateTitles.ReturnedCurrenciesTitle, replyMarkup: keyboard);
            await _manager.CacheMessage(chatId.ToString(), msg.MessageId.ToString());
        }

		private InlineKeyboardMarkup GetReturnedCurrenciesKeyboard(IEnumerable<CurrencyDto> currencies) {
			var dictionarifiedValues = GetDictionarifiedValues(currencies);
			return RatesMenu.GetKeyboard(dictionarifiedValues);
		}

		private IDictionary<string, string> GetDictionarifiedValues(IEnumerable<CurrencyDto> currencies) {
			var dictionary = new Dictionary<string, string>();
			foreach (var currency in currencies) {
				var key = _cbPrefix + currency.Id.ToString();
				var value = currency.Code == null ? "none" : currency.Code;
				dictionary.TryAdd(key, value);
			}
			return dictionary;
		}
	}
}
