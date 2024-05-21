using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Constants;
using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Application.Views.InlineMarkups;
using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExchangeOffice.Application.Handlers {
	[TextMessageHandler(MenuTitles.Rates)]
	public class RatesMessageHandler : ITextMessageHandler {
		private ITelegramBotClient _bot;
		private IRateManager _rateManager;
		public RatesMessageHandler(IManagerProvider provider) {
			_bot = provider.GetTelegramBotClient();
			_rateManager = provider.GetRateManager();
		}

		public async Task ExecuteAsync(Update request) {
			var chatId = request?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}
			var rates = await _rateManager.GetRates(chatId.ToString());
			var acceptedCurrencies = await _rateManager.GetAcceptedCurrencies(chatId.ToString());
			var currdict = new Dictionary<string, string>();
			foreach(var curr in acceptedCurrencies) {
				var key = "acceptedcurrency|" + curr.Id.ToString();
				currdict.TryAdd(key, curr.Code);
			}
			var buttons = RatesMenu.GetButtons(currdict);
			await _bot.SendTextMessageAsync(chatId, text: "test", replyMarkup: buttons);
		}
	}
}
