using CurrencyApplication.App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CurrencyApplication.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> logger;
        private readonly IApiKeyRepository apiKeyRepository;

        public LoginController(ILogger<LoginController> logger,
            IApiKeyRepository apiKeyRepository)
        {
            this.logger = logger;
            this.apiKeyRepository = apiKeyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetApiKey()
        {
            var apiKey = Guid.NewGuid();
            logger.LogInformation($"An attempt for generation new API KEY. Generated value {apiKey}");
            await apiKeyRepository.AddAsync(apiKey);
            return new ObjectResult(apiKey);
        }
    }
}
