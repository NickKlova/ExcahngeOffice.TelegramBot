using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ExchangeOffice.Cache.Extensions;
using Microsoft.AspNetCore.Builder;
using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Application.Managers;
using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Extensions.Providers;
using ExchangeOffice.Application.Extensions.Middlewares;

namespace ExchangeOffice.Core.Extensions {
	public static class ApplicationExtensions {
		public static void AddApplicationLayer(this IServiceCollection services) {
			services.AddCacheLayer();
			services.AddCoreLayer();
			var assembly = Assembly.GetExecutingAssembly();
			var handlerTypes = assembly.GetTypes()
				.Where(t => t.GetCustomAttributes(typeof(TextMessageHandlerAttribute), true).Length > 0);

			foreach (var handlerType in handlerTypes) {
				var attribute = handlerType.GetCustomAttribute<TextMessageHandlerAttribute>();
				if (attribute != null) {
					services.AddSingleton(typeof(ITextMessageHandler), handlerType);
				}
			}
			var callbackTypes = assembly.GetTypes()
				.Where(t => t.GetCustomAttributes(typeof(CallbackMessageHandlerAttribute), true).Length > 0);

			foreach (var handlerType in callbackTypes) {
				var attribute = handlerType.GetCustomAttribute<CallbackMessageHandlerAttribute>();
				if (attribute != null) {
					services.AddSingleton(typeof(ICallbackMessageHandler), handlerType);
				}
			}
			services.AddSingleton<IContactManager, ContactManager>();
			services.AddSingleton<IManagerProvider, ManagerProvider>();
		}

		public static void UseApplicationLayer(this IApplicationBuilder app) {
			app.UseMiddleware<HandlersMiddleware>();
			app.UseMiddleware<StepperMiddleware>();
		}
	}
}
