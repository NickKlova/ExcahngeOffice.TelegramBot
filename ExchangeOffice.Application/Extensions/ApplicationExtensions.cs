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
using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Application.Managers;
using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Extensions.Providers;

namespace ExchangeOffice.Core.Extensions {
	public static class ApplicationExtensions {
		public static void AddApplicationLayer(this IServiceCollection services) {
			services.AddCacheLayer();
			var assembly = Assembly.GetExecutingAssembly();
			var handlerTypes = assembly.GetTypes()
				.Where(t => t.GetCustomAttributes(typeof(TextMessageHandlerAttribute), true).Length > 0);

			foreach (var handlerType in handlerTypes) {
				var attribute = handlerType.GetCustomAttribute<TextMessageHandlerAttribute>();
				if (attribute != null) {
					services.AddSingleton(typeof(IMessageHandler), handlerType);
				}
			}
			services.AddSingleton<IContactManager, ContactManager>();
			services.AddSingleton<IManagerProvider, ManagerProvider>();
		}

		public static void UseApplicationLayer(this IApplicationBuilder app) {
			app.UseMiddleware<StepperMiddleware>();
		}
	}
}
