using System;
using System.Threading.Tasks;
using AutoMapper;
using CurrencyApplication.Api.ActionFilters;
using CurrencyApplication.App.Models;
using CurrencyApplication.App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyApplication.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ServiceFilter(typeof(ApiKeyAuthorizationAsyncActionFilter))]
    public class ExchangeRateController : ControllerBase
    {
        private readonly ILogger<ExchangeRateController> logger;
        private readonly IMapper mapper;
        private readonly IExchangeRateService exchangeRateService;

        public ExchangeRateController(ILogger<ExchangeRateController> logger,
            IMapper mapper,
            IExchangeRateService exchangeRateService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.exchangeRateService = exchangeRateService;
        }

        [HttpPost]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetExchangeRates([FromBody] Requests.GetExchangeRateRequest request)
        {
            logger.LogInformation($"Attempt to fetch data from api: APIKEY: {request.ApiKey}, date range: {request.StartTime}-{request.EndTime}, exchangeRates : {request.CurrencyCodesDescription}");

            if (request.StartTime > DateTime.Now)
                return NotFound("Not found");

            return new ObjectResult(await exchangeRateService.GetRangeAsync(mapper.Map<ExchangeRateFetchModel>(request)));
        }
    }
}
