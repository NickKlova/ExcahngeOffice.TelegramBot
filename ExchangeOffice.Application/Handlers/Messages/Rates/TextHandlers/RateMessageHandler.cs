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

namespace ExchangeOffice.Application.Handlers.Messages.Rates.TextHandlers
{
    [TextMessageHandler(MenuTitles.Rates)]
    public class RateMessageHandler : ITextMessageHandler
    {
        private ITelegramBotClient _bot;
        private IRateManager _rateManager;
        private string _cbPrefix;
        public RateMessageHandler(IManagerProvider provider)
        {
            _bot = provider.GetTelegramBotClient();
            _rateManager = provider.GetRateManager();
            _cbPrefix = Callbacks.AcceptedCurrencies + "|";
        }

        public async Task ExecuteAsync(Update request)
        {
            var chatId = request?.Message?.Chat.Id;
            if (chatId == null)
            {
                return;
            }

            var key = chatId.ToString();
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            var loadingMessage = await _bot.SendStickerAsync(chatId, sticker: InputFile.FromFileId(Stickers.LoadingSticker));
            var rates = await _rateManager.GetRatesAsync(key);
            await _bot.DeleteMessageAsync(chatId, loadingMessage.MessageId);

            if (rates.Count() == 0)
            {
                await _bot.SendTextMessageAsync(chatId, text: RateTitles.ExchangeHaventRatesTitle, replyMarkup: MainMenu.Buttons);
            }

            var acceptedCurrencies = await _rateManager.GetAcceptedCurrenciesAsync(key);
            if (acceptedCurrencies.Count() == 0)
            {
                await _bot.SendTextMessageAsync(chatId, text: RateTitles.ExchangeHaventRatesTitle, replyMarkup: MainMenu.Buttons);
            }

            var keyboard = GetAcceptedCurrenciesKeyboard(acceptedCurrencies);
            var msg = await _bot.SendTextMessageAsync(chatId, text: RateTitles.AcceptedRatesTitle, replyMarkup: keyboard);
			await _rateManager.CacheMessage(chatId.ToString(), msg.MessageId.ToString());
		}

		private InlineKeyboardMarkup GetAcceptedCurrenciesKeyboard(IEnumerable<CurrencyDto> currencies)
        {
            var dictionarifiedValues = GetDictionarifiedValues(currencies);
            return RatesMenu.GetKeyboard(dictionarifiedValues);
        }

        private IDictionary<string, string> GetDictionarifiedValues(IEnumerable<CurrencyDto> currencies)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var currency in currencies)
            {
                var key = _cbPrefix + currency.Id.ToString();
                var value = currency.Code == null ? "none" : currency.Code;
                dictionary.TryAdd(key, value);
            }
            return dictionary;
        }
    }
}
