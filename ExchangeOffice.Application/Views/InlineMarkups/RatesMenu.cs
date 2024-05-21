using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangeOffice.Application.Views.InlineMarkups {
	public static class RatesMenu {
		public static InlineKeyboardMarkup GetKeyboard(IDictionary<string, string> values) {
			var keyboard = new List<List<InlineKeyboardButton>>();
			foreach (var key in values.Keys) {
				var row = new List<InlineKeyboardButton>();
				var value = values[key];
                var button = InlineKeyboardButton.WithCallbackData(text: value, callbackData: key);
                row.Add(button);
				keyboard.Add(row);
            }
			return new InlineKeyboardMarkup(keyboard);
		}
	}
}
