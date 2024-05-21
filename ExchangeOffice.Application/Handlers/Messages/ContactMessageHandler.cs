using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Constants;
using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Handlers.Messages.Abstractions;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Common.Models;
using ExchangeOffice.Core.Views.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExchangeOffice.Application.Handlers.Messages {
	[TextMessageHandler(MenuTitles.MyContact)]
	public class ContactMessageHandler : BaseMessageHandler, ITextMessageHandler {
		private readonly ITelegramBotClient _bot;
		private readonly IContactManager _contactManager;
		public ContactMessageHandler(IManagerProvider managerProvider) : base(managerProvider) {
			_bot = managerProvider.GetTelegramBotClient();
			_contactManager = managerProvider.GetContactManager();
		}

		

		[TextStepper(MenuTitles.MyContact, 1)]
		public async Task ExecuteAsync(Update request) {
			var chatId = request?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}
			var config = new StepperInfo() {
				CurrentStep = 1,
				StepsCount = 4,
				Name = MenuTitles.MyContact,
			};
			await ConfigureStepperAsync(chatId, config);

			await _bot.SendTextMessageAsync(chatId, "Enter your full name:", replyMarkup: MainMenu.Buttons);
		}

		[TextStepper(MenuTitles.MyContact, 2)]
		public async Task FillUsernameParameter (Update request) {
			var chatId = request?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}

			await _bot.SendTextMessageAsync(chatId, "Enter your phone number:", replyMarkup: MainMenu.Buttons);
			//await _contactManager.FillFullnameAsync()
			await NextOrFinishStepAsync(chatId);    
		}

		[TextStepper(MenuTitles.MyContact, 3)]
		public async Task FillPhoneNumber(Update request) {
			var chatId = request?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}

			await _bot.SendTextMessageAsync(chatId, "Enter your email:", replyMarkup: MainMenu.Buttons);
			await NextOrFinishStepAsync(chatId);
		}

		[TextStepper(MenuTitles.MyContact, 4)]
		public async Task FillEmail(Update request) {
			var chatId = request?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}

			await _bot.SendTextMessageAsync(chatId, "Success", replyMarkup: MainMenu.Buttons);
			await NextOrFinishStepAsync(chatId);
		}
	}
}
