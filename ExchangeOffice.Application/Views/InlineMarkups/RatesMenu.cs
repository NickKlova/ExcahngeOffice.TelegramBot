using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangeOffice.Application.Views.InlineMarkups {
	public static class RatesMenu {
        public static InlineKeyboardMarkup GetButtons(IDictionary<string, string> values) {
			var kbButtons = new List<List<InlineKeyboardButton>>();

			var list = new List<InlineKeyboardButton>();
			foreach (var key in values.Keys) {
                var value = values[key];
                var button = InlineKeyboardButton.WithCallbackData(text: value, callbackData: key);
                list.Add(button);
            }
            kbButtons.Add(list);
			InlineKeyboardMarkup keyboard = new(kbButtons);
            return keyboard;
		}
	}
}
