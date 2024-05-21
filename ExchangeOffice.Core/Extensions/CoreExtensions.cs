using ExchangeOffice.Core.Clients;
using ExchangeOffice.Core.Clients.Interfaces;
using ExchangeOffice.Core.DAO.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeOffice.Core.Extensions {
	public static class CoreExtensions {
		public static void AddCoreLayer(this IServiceCollection services) {
			services.AddAutoMapper(typeof(AutoMapperProfile));
			services.AddSingleton<IContactService, ContactService>();
		}
	}
}
