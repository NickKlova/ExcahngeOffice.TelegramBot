using AutoMapper;
using ExchangeOffice.Common.Models;

namespace ExchangeOffice.Core.DAO.Mappers {
	public class AutoMapperProfile : Profile {
		public AutoMapperProfile() {
			CreateContactMapper();
		}

		private void CreateContactMapper() {
			CreateMap<ContactDto, Contact>();
		}
	}
}
