using CurrencyApplication.Api.Requests;
using CurrencyApplication.App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CurrencyApplication.Api.ActionFilters
{
    public class ApiKeyAuthorizationAsyncActionFilter : IAsyncActionFilter
    {
        private readonly IApiKeyRepository apiKeyRepository;
        private readonly ILogger<ApiKeyAuthorizationAsyncActionFilter> logger;

        public ApiKeyAuthorizationAsyncActionFilter(IApiKeyRepository apiKeyRepository,
            ILogger<ApiKeyAuthorizationAsyncActionFilter> logger)
        {
            this.apiKeyRepository = apiKeyRepository;
            this.logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isRequestFound = context.ActionArguments.TryGetValue("request", out var requestObject);
            var request = (GetExchangeRateRequest)requestObject;
            if (isRequestFound && request.ApiKey == null)
            {
                logger.LogError($"Detected request without ApiKey.");
                context.Result = new UnauthorizedObjectResult("Missing api key in request");
                return;
            }

            var isApiKeyMatched = await apiKeyRepository.ExistsAsync(Guid.Parse(request.ApiKey));

            if (!isApiKeyMatched)
            {
                logger.LogError($"Detected request with ApiKey ({request.ApiKey}) not presented in database.");
                context.Result = new UnauthorizedObjectResult("Wrong api key!");
                return;
            }

            await next();
        }
    }
}
