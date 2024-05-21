using ExchangeOffice.Core.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOffice.Core.Services.Interfaces {
	public interface IRateService {
		public Task<IEnumerable<Rate>> GetRates();
		}
	}
