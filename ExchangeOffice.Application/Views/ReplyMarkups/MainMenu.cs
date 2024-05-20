using ExchangeOffice.Application.Constants;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangeOffice.Core.Views.ReplyMarkups {
	public static class MainMenu {
		public static readonly ReplyKeyboardMarkup Buttons = new(
		new[]
		{
			new KeyboardButton[] { MenuTitles.MyContact },
			new KeyboardButton[] { MenuTitles.Reservations, MenuTitles.Rates },
			new KeyboardButton[] { MenuTitles.Faq },
			new KeyboardButton[] { MenuTitles.Support }
		}) 
		{ ResizeKeyboard = true};
	}
}
