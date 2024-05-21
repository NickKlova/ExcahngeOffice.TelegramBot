using ExchangeOffice.Common.Models;

namespace ExchangeOffice.Core.Clients.Interfaces {
	public interface IContactService {
		public Task<ContactDto> CreateContactAsync(string cacheId, ContactDto data);
		public Task<ContactDto> GetContactAsync(string cacheId);
		}
	}
