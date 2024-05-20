using ExchangeOffice.Application.Managers.Interfaces;
using Telegram.Bot;

namespace ExchangeOffice.Application.Extensions.Providers.Interfaces {
	public interface IManagerProvider {
		public IContactManager GetContactManager();
		public ITelegramBotClient GetTelegramBotClient();
	}
}
