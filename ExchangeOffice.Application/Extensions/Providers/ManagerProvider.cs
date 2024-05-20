using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Managers.Interfaces;
using Telegram.Bot;

namespace ExchangeOffice.Application.Extensions.Providers {
	public class ManagerProvider : IManagerProvider {
		private readonly IServiceProvider _serviceProvider;
		public ManagerProvider(IServiceProvider serviceProvider) {
			_serviceProvider = serviceProvider;
		}

		public ITelegramBotClient GetTelegramBotClient() {
			var manager = _serviceProvider.GetService(typeof(ITelegramBotClient));
			if (manager == null) {
				throw new Exception("test");
			}

			var typedManager = (ITelegramBotClient)manager;
			return typedManager;
		}

		public IContactManager GetContactManager() {
			var manager = _serviceProvider.GetService(typeof(IContactManager));
			if (manager == null) {
				throw new Exception("test");
			}

			var typedManager = (IContactManager)manager;
			return typedManager;
		}
	}
}
