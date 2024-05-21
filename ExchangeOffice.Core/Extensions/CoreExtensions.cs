using ExchangeOffice.Core.Clients;
using ExchangeOffice.Core.Clients.Interfaces;
using ExchangeOffice.Core.DAO.Mappers;
using ExchangeOffice.Core.Services;
using ExchangeOffice.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeOffice.Core.Extensions {
	public static class CoreExtensions {
		public static void AddCoreLayer(this IServiceCollection services) {
			services.AddAutoMapper(typeof(AutoMapperProfile));
			services.AddSingleton<IContactService, ContactService>();
			services.AddSingleton<IRateService, RateService>();
		}
	}
}
