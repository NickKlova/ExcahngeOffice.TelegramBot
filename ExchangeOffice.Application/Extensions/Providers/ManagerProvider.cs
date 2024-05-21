using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Cache.Clients.Interfaces;
using Telegram.Bot;

namespace ExchangeOffice.Application.Extensions.Providers {
	public class ManagerProvider : IManagerProvider {
		private readonly IServiceProvider _serviceProvider;
		public ManagerProvider(IServiceProvider serviceProvider) {
			_serviceProvider = serviceProvider;
		}

		public ICacheClient GetCacheClient() {
			var manager = _serviceProvider.GetService(typeof(ICacheClient));
			if (manager == null) {
				throw new Exception("test");
			}

			var typedManager = (ICacheClient)manager;
			return typedManager;
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

		public IRateManager GetRateManager() {
			var manager = _serviceProvider.GetService(typeof(IRateManager));
			if (manager == null) {
				throw new Exception("test");
			}

			var typedManager = (IRateManager)manager;
			return typedManager;
		}
	}
}
