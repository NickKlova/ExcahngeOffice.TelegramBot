using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Cache.Clients.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Requests.Abstractions;

namespace ExchangeOffice.Application.Managers {
	public class ContactManager : IContactManager {
		private readonly ICacheClient _cache;
		public ContactManager(ICacheClient cache) {
			_cache = cache;
		}
		public void SetData(string id, string str) {
			_cache.SetAsync(id, str).Wait();
		}
	} 
}
