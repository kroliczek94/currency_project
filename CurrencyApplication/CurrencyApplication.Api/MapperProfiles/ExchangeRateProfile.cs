using AutoMapper;
using CurrencyApplication.App.Models;

namespace CurrencyApplication.Api.MapperProfiles
{
    public class ExchangeRateProfile : Profile
    {
        public ExchangeRateProfile()
        {
            CreateMap<Requests.GetExchangeRateRequest, ExchangeRateFetchModel>();
        }
    }
}
