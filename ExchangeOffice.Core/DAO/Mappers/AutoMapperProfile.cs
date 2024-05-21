using AutoMapper;
using ExchangeOffice.Common.Models;

namespace ExchangeOffice.Core.DAO.Mappers {
	public class AutoMapperProfile : Profile {
		public AutoMapperProfile() {
			CreateContactMapper();
			CreateRateMapper();
		}

		private void CreateContactMapper() {
			CreateMap<ContactDto, Contact>();
		}

		private void CreateRateMapper() {
			CreateMap<Currency, CurrencyDto>();

			CreateMap<Rate, RateDto>()
							.ForMember(dest => dest.BaseCurrency, opt => opt.MapFrom(src => src.BaseCurrency))
							.ForMember(dest => dest.TargetCurrency, opt => opt.MapFrom(src => src.TargetCurrency));
			CreateMap<IEnumerable<Rate>, IEnumerable<RateDto>>()
				.ConvertUsing((src, dest, context) => src.Select(x => context.Mapper.Map<RateDto>(x)));
		}
	}
}
