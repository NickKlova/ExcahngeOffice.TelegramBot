namespace ExchangeOffice.Application.Attributes {
	public class CallbackMessageHandlerAttribute : Attribute {
		public string Text { get; set; }
		public CallbackMessageHandlerAttribute(string text) {
			Text = text;
		}
	}
}
