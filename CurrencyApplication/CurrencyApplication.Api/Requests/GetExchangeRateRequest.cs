using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyApplication.Api.Requests
{
    public class GetExchangeRateRequest
    {
        public Dictionary<string, string> CurrencyCodes { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ApiKey { get; set; }

        public string CurrencyCodesDescription => string.Join(";",CurrencyCodes.Select(rate => $"[{rate.Key}:{rate.Value}]"));
    }
}
