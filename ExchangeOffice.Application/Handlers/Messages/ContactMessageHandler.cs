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
			var key = chatId.ToString();
			if(string.IsNullOrEmpty(key)) { 
				return; 
			}
			var contact = await _contactManager.GetContactAsync(key);
			if(contact != null) {
				var registeredContactMessage = GetRegisteredContactMessageText(contact);
				await _bot.SendTextMessageAsync(chatId, registeredContactMessage, replyMarkup: MainMenu.Buttons);
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
			var key = chatId.ToString();
			if (string.IsNullOrEmpty(key)) {
				return;
			}

			var text = request?.Message?.Text;
			await _contactManager.FillFullnameAsync(key, text);
			await _bot.SendTextMessageAsync(chatId, "Enter your phone number:", replyMarkup: MainMenu.Buttons);
			await NextOrFinishStepAsync(chatId);    
		}

		[TextStepper(MenuTitles.MyContact, 3)]
		public async Task FillPhoneNumber(Update request) {
			var chatId = request?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}
			var key = chatId.ToString();
			if (string.IsNullOrEmpty(key)) {
				return;
			}

			var text = request?.Message?.Text;
			await _contactManager.FillPhoneAsync(key, text);
			await _bot.SendTextMessageAsync(chatId, "Enter your email:", replyMarkup: MainMenu.Buttons);
			await NextOrFinishStepAsync(chatId);
		}

		[TextStepper(MenuTitles.MyContact, 4)]
		public async Task FillEmail(Update request) {
			var chatId = request?.Message?.Chat.Id;
			if (chatId == null) {
				return;
			}
			var key = chatId.ToString();
			if (string.IsNullOrEmpty(key)) {
				return;
			}
			var msg = await _bot.SendTextMessageAsync(chatId, "Wait a moment!", replyMarkup: MainMenu.Buttons);
			var text = request?.Message?.Text;
			await _contactManager.FillEmailAsync(key, text);
			await _contactManager.CreateContactAsync(key);
			await _bot.DeleteMessageAsync(chatId, msg.MessageId);
			await _bot.SendTextMessageAsync(chatId, "Successfully register your contact!", replyMarkup: MainMenu.Buttons);
			await NextOrFinishStepAsync(chatId);
		}

		private string GetRegisteredContactMessageText(ContactDto entity) {
			return $"Unique contact key: {entity.Id}\n" +
				$"Full name: {entity.FullName}\n" +
				$"Email: {entity.Email}\n" +
				$"Phone: {entity.Phone}\n"; 
		}
	}
}
