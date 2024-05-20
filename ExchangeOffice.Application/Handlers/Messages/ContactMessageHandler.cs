using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Constants;
using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Common.Models;
using ExchangeOffice.Core.Views.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExchangeOffice.Application.Handlers.Messages {
	[TextMessageHandler(MenuTitles.MyContact)]
	public class ContactMessageHandler : IMessageHandler {
		private readonly ITelegramBotClient _bot;
		private readonly IContactManager _contactManager;
		public ContactMessageHandler(IManagerProvider managerProvider) {
			_bot = managerProvider.GetTelegramBotClient();
			_contactManager = managerProvider.GetContactManager();
		}

		private async Task ConfigureStepperAsync(object key) {
			var stringKey = key.ToString();
			if (stringKey == null) {
				throw new Exception("Key can't be null!");
			}
			var config = new StepperInfo() {
				CurrentStep = 1,
				StepsCount = 4,
				Name = MenuTitles.MyContact,
			};
			await _contactManager.NextOrFinishStepAsync(stringKey, config);
		}

		[TextStepper(MenuTitles.MyContact, 1)]
		public async Task ExecuteAsync(Update request) {
			var chatId = request?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}
			await ConfigureStepperAsync(chatId);

			await _bot.SendTextMessageAsync(chatId, "Enter your full name:", replyMarkup: MainMenu.Buttons);
		}

		[TextStepper(MenuTitles.MyContact, 2)]
		public async Task FillUsernameParameter (Update request) {
			var chatId = request?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}
			if (request?.Message?.Text == MenuTitles.MyContact) {
				await _contactManager.DeleteStepperAsync(chatId);
				return;
			}

			await _bot.SendTextMessageAsync(chatId, "Enter your phone number:", replyMarkup: MainMenu.Buttons);
			await _contactManager.NextOrFinishStepAsync(chatId);    
		}

		[TextStepper(MenuTitles.MyContact, 3)]
		public async Task FillPhoneNumber(Update request) {
			var chatId = request?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}
			if (request?.Message?.Text == MenuTitles.MyContact) {
				await _contactManager.DeleteStepperAsync(chatId);
				return;
			}

			await _bot.SendTextMessageAsync(chatId, "Enter your email:", replyMarkup: MainMenu.Buttons);
			await _contactManager.NextOrFinishStepAsync(chatId);
		}

		[TextStepper(MenuTitles.MyContact, 4)]
		public async Task FillEmail(Update request) {
			var chatId = request?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}
			if (request?.Message?.Text == MenuTitles.MyContact) {
				await _contactManager.DeleteStepperAsync(chatId);
				return;
			}

			await _bot.SendTextMessageAsync(chatId, "Success", replyMarkup: MainMenu.Buttons);
			await _contactManager.NextOrFinishStepAsync(chatId);
		}
	}
}
