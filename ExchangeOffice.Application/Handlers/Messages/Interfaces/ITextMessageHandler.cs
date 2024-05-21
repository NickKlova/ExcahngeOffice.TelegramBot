using Telegram.Bot.Types;

namespace ExchangeOffice.Application.Handlers.Messages.Interfaces {
	public interface ITextMessageHandler {
		public Task ExecuteAsync(Update request);
	}
}
