using AutoMapper;
using CurrencyApplication.App.Models;

namespace CurrencyApplication.Infrastructure.MapperProfiles
{
    public class ExchangeRateProfile : Profile
    {
        public ExchangeRateProfile()
        {
            CreateMap<Database.Entities.ExchangeRate, ExchangeRate>();
            CreateMap<ExchangeRate, Database.Entities.ExchangeRate>();
        }
    }
}
