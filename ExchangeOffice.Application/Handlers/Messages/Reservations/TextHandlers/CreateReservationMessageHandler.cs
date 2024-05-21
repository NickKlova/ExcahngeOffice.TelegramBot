using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Constants;
using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Application.Views.InlineMarkups;
using ExchangeOffice.Common.Models;
using ExchangeOffice.Core.Views.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangeOffice.Application.Handlers.Messages.Reservations.TextHandlers
{
    [TextMessageHandler(MenuTitles.NewReservation)]
    public class CreateReservationMessageHandler : ITextMessageHandler
    {
		private ITelegramBotClient _bot;
		private IReservationManager _reservationManager;
		private string _cbPrefix;
		public CreateReservationMessageHandler(IManagerProvider provider) {
			_bot = provider.GetTelegramBotClient();
			_reservationManager = provider.GetReservationManager();
			_cbPrefix = Callbacks.ReservationAcceptedCurrencies + "|";
		}

		public async Task ExecuteAsync(Update request) {
			var chatId = request?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}

			var key = chatId.ToString();
			if (string.IsNullOrEmpty(key)) {
				return;
			}

			var loadingMessage = await _bot.SendStickerAsync(chatId, sticker: InputFile.FromFileId(Stickers.LoadingSticker));
			var rates = await _reservationManager.GetRatesAsync(key);
			await _bot.DeleteMessageAsync(chatId, loadingMessage.MessageId);

			if (rates.Count() == 0) {
				await _bot.SendTextMessageAsync(chatId, text: RateTitles.ExchangeHaventRatesTitle, replyMarkup: MainMenu.Buttons);
			}

			var acceptedCurrencies = await _reservationManager.GetAcceptedCurrenciesAsync(key);
			if (acceptedCurrencies.Count() == 0) {
				await _bot.SendTextMessageAsync(chatId, text: RateTitles.ExchangeHaventRatesTitle, replyMarkup: MainMenu.Buttons);
			}

			var keyboard = GetAcceptedCurrenciesKeyboard(acceptedCurrencies);
			var msg = await _bot.SendTextMessageAsync(chatId, text: RateTitles.AcceptedRatesTitle, replyMarkup: keyboard);
			await _reservationManager.CacheMessage(chatId.ToString(), msg.MessageId.ToString());
		}

		private InlineKeyboardMarkup GetAcceptedCurrenciesKeyboard(IEnumerable<CurrencyDto> currencies) {
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
