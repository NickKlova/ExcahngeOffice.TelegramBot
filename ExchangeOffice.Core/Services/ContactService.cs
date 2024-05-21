using AutoMapper;
using ExchangeOffice.Common.Models;
using ExchangeOffice.Core.Clients.Interfaces;
using ExchangeOffice.Core.DAO;
using ExchangeOffice.Core.Services.Abstractions;

namespace ExchangeOffice.Core.Clients {
	public class ContactService : BaseService, IContactService {
		private readonly IMapper _mapper;
		public ContactService(IMapper mapper) {
			_mapper = mapper;
		}
		public async Task<ContactDto> CreateContactAsync(ContactDto data) {
			var dao = _mapper.Map<Contact>(data);
			dao.IsBlacklist = false;
			var response = await PostAsync("", dao);
			if (response == null) {
				throw new Exception("test");
			}
			return data; 
		}
	}
}
