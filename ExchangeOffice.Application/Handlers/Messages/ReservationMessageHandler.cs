using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Constants;
using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Handlers.Messages.Abstractions;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Application.Views.ReplyMarkups;
using ExchangeOffice.Common.Models;
using ExchangeOffice.Core.Views.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExchangeOffice.Application.Handlers.Messages {
	[TextMessageHandler(MenuTitles.Reservations)]
	public class ReservationMessageHandler : BaseMessageHandler, ITextMessageHandler {
		private readonly ITelegramBotClient _bot;
		private readonly IContactManager _contactManager;
		public ReservationMessageHandler(IManagerProvider managerProvider) : base(managerProvider) {
			_bot = managerProvider.GetTelegramBotClient();
			_contactManager = managerProvider.GetContactManager();
		}

		[TextStepper(MenuTitles.Reservations, 1)]
		public async Task ExecuteAsync(Update request) {
			var chatId = request?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}
			var config = new StepperInfo() {
				CurrentStep = 1,
				StepsCount = 2,
				Name = MenuTitles.Reservations,
			};
			await ConfigureStepperAsync(chatId, config);
			await _bot.SendTextMessageAsync(chatId, "question 1", replyMarkup: ReservationMenu.Buttons);
		}


		[TextStepper(MenuTitles.Reservations, 2)]
		public async Task FillUsernameParameter(Update request) {
			var chatId = request?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}

			await _bot.SendTextMessageAsync(chatId, "question 2", replyMarkup: MainMenu.Buttons);
			await NextOrFinishStepAsync(chatId);
		}
	}
}
