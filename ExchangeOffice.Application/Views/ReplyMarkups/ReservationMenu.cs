using ExchangeOffice.Application.Constants;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangeOffice.Application.Views.ReplyMarkups {
	public static class ReservationMenu {
		public static readonly ReplyKeyboardMarkup Buttons = new(
		new[] {
			new KeyboardButton[] { MenuTitles.MyReservations },
			new KeyboardButton[] { MenuTitles.Reservations },
			new KeyboardButton[] { MenuTitles.Back }
		}) { ResizeKeyboard = true };
	}
}
