using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using ExchangeOffice.Common.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Reflection;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

namespace ExchangeOffice.Application.Extensions.Middlewares {
	public class HandlersMiddleware {
		private readonly RequestDelegate _next;
		private readonly Dictionary<string, IMessageHandler> _handlers;
		public HandlersMiddleware(RequestDelegate next, IEnumerable<IMessageHandler> handlers) {
			_next = next;
			_handlers = handlers
				.Select(h => new { Handler = h, Attribute = h.GetType().GetCustomAttribute<TextMessageHandlerAttribute>() })
				.Where(x => x.Attribute != null)
				.ToDictionary(x => x.Attribute!.Text, x => x.Handler);
		}

		public async Task InvokeAsync(HttpContext context) {
			context.Request.EnableBuffering();
			var update = await GetUpdateFromRequest(context.Request.Body);

			if (update == null) {
				return;
			}

			var text = update?.Message?.Text;
			if (string.IsNullOrEmpty(text)) {
				return;
			}

			if (_handlers.TryGetValue(text, out var handler)) {
				await handler.ExecuteAsync(update!);
				context.Request.Headers["handlerexecuted"] = "yes";
			}

			await _next(context);
		}

		private async Task<Update?> GetUpdateFromRequest(Stream requestBody) {
			var body = await GetRequestBody(requestBody);
			return JsonConvert.DeserializeObject<Update>(body);
		}

		private async Task<string> GetRequestBody(Stream body) {
			var data = await new StreamReader(body, leaveOpen: true).ReadToEndAsync();
			if (body.CanSeek)
				body.Position = 0;
			return data;
		}
	}
}
