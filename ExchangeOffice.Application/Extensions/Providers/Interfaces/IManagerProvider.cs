using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Cache.Clients.Interfaces;
using Telegram.Bot;

namespace ExchangeOffice.Application.Extensions.Providers.Interfaces {
	public interface IManagerProvider {
		public IContactManager GetContactManager();
		public ITelegramBotClient GetTelegramBotClient();
		public ICacheClient GetCacheClient();
		public IRateManager GetRateManager();
		public IReservationManager GetReservationManager();
		}
	}
