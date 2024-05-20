using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Constants;
using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using ExchangeOffice.Cache.Clients.Interfaces;
using ExchangeOffice.Common.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using Telegram.Bot.Types;

namespace ExchangeOffice.Application.Handlers.Messages {
	[TextMessageHandler(MenuTitles.MyContact)]
	public class ContactMessageHandler : IMessageHandler {
		private readonly IManagerProvider _managerProvider;
		public ContactMessageHandler(IManagerProvider managerProvider) {
			_managerProvider = managerProvider;
		}

		[TextStepper(MenuTitles.MyContact, 1)]
		public void Execute(Update request) {
			var stepprInfo = new StepperInfo() {
				CurrentStep = 1,
				StepsCount = 2,
				Name = MenuTitles.MyContact
			};
			var str = JsonConvert.SerializeObject(stepprInfo);

			_managerProvider.GetContactManager().SetData(request.Message.Chat.Id.ToString(), str);
			Console.WriteLine("test");
		}

		[TextStepper(MenuTitles.MyContact, 2)]
		public void Execute2(Update request) {
			Console.WriteLine("test2");
		}
	}
}
