using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using ExchangeOffice.Cache.Extensions;
using Microsoft.AspNetCore.Builder;

namespace ExchangeOffice.Core.Extensions {
	public static class ApplicationExtensions {
		public static void AddApplicationLayer(this IServiceCollection services) {
			//services.AddSingleton<ITelegramBotClient>(options => {
			//	var client = new TelegramBotClient("6700447814:AAG-ynj_oHoZ9mEeW8kORRCSXDl0Aewf2i0");
			//	client.SetWebhookAsync("https://9d88-176-39-34-13.ngrok-free.app/api/telegam/update");
			//	return client;
			//});
			services.AddCacheLayer();
			var assembly = Assembly.GetExecutingAssembly();
			var handlerTypes = assembly.GetTypes()
				.Where(t => t.GetCustomAttributes(typeof(TextMessageHandlerAttribute), true).Length > 0);

			foreach (var handlerType in handlerTypes) {
				var attribute = handlerType.GetCustomAttribute<TextMessageHandlerAttribute>();
				if (attribute != null) {
					var instance = Activator.CreateInstance(handlerType);
					if (instance != null) {
						var interfacedInstance = (IMessageHandler)instance;
						services.AddSingleton<IMessageHandler>(interfacedInstance);
					}
				}
			}

		}

		public static void UseApplicationLayer(this IApplicationBuilder app) {
			app.UseMiddleware<StepperMiddleware>();
		}
	}
}
