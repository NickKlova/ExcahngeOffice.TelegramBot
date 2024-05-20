using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangeOffice.Core.Views.ReplyMarkups {
	public static class Menu {
		private static readonly ReplyKeyboardMarkup Buttons = new(new[]
		{
			new KeyboardButton[] { "Help me" },
			new KeyboardButton[] { "Call me ☎️" },
		});
	}
}
