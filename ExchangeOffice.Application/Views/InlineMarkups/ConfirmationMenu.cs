using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangeOffice.Application.Views.InlineMarkups {
	public static class ConfirmationMenu {
		public static InlineKeyboardMarkup Keyboard = new(new[]
		{
			// first row
			new []
			{
				InlineKeyboardButton.WithCallbackData(text: "Confirm", callbackData: "Confirm"),
				InlineKeyboardButton.WithCallbackData(text: "Decline", callbackData: "Decline"),
			}
		});
	}
}
