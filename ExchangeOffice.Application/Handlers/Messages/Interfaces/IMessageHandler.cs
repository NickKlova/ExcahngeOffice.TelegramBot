using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ExchangeOffice.Application.Handlers.Messages.Interfaces {
	public interface IMessageHandler {
		public void Execute(Update request);
	}
}
