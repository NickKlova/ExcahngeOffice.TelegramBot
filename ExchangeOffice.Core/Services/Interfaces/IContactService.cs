using ExchangeOffice.Common.Models;

namespace ExchangeOffice.Core.Clients.Interfaces {
	public interface IContactService {
		public Task<ContactDto> CreateContactAsync(ContactDto data);
	}
}
