using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOffice.Application.Attributes {
	public class CallbackMessageHandlerAttribute : Attribute {
		public string Text { get; set; }
		public CallbackMessageHandlerAttribute(string text) {
			Text = text;
		}
	}
}
