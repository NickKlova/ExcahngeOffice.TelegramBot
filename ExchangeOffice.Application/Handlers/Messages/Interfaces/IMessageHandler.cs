using Telegram.Bot.Types;

namespace ExchangeOffice.Application.Handlers.Messages.Interfaces {
	public interface IMessageHandler {
		public Task ExecuteAsync(Update request);
	}
}
