using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Managers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOffice.Application.Extensions.Providers {
	public class ManagerProvider : IManagerProvider {
		private readonly IServiceProvider _serviceProvider;
		public ManagerProvider(IServiceProvider serviceProvider) {
			_serviceProvider = serviceProvider;
		}

		public IContactManager GetContactManager() {
			var manager = _serviceProvider.GetService(typeof(IContactManager));
			if (manager == null) {
				throw new Exception("test");
			}

			var typedManager = (IContactManager)manager;
			return typedManager;
		}
	}
}
