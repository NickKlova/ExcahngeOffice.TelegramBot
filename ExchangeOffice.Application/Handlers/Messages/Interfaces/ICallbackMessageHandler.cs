using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ExchangeOffice.Application.Handlers.Messages.Interfaces {
	public interface ICallbackMessageHandler {
		public Task ExecuteAsync(Update request);
	}
}
