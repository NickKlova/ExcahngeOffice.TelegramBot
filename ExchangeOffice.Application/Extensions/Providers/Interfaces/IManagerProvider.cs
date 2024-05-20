using ExchangeOffice.Application.Managers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOffice.Application.Extensions.Providers.Interfaces {
	public interface IManagerProvider {
		public IContactManager GetContactManager();
	}
}
