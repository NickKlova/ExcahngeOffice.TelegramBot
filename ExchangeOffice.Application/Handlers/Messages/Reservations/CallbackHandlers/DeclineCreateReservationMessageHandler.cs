using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Application.Views.InlineMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExchangeOffice.Application.Handlers.Messages.Reservations.CallbackHandlers {
	[CallbackMessageHandler("Decline")]
	public class DeclineCreateReservationMessageHandler : ICallbackMessageHandler {
		private readonly IReservationManager _manager;
		private readonly ITelegramBotClient _bot;
		public DeclineCreateReservationMessageHandler(IManagerProvider managerProvider) {
			_manager = managerProvider.GetReservationManager();
			_bot = managerProvider.GetTelegramBotClient();
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

			var lastMessageId = await _manager.GetLastBotMessageId(chatId.ToString());
			await _bot.DeleteMessageAsync(chatId, messageId: Convert.ToInt32(lastMessageId));
			var msg = await _bot.SendTextMessageAsync(chatId, "Declined", replyMarkup: ConfirmationMenu.Keyboard);
		}
	}
}
