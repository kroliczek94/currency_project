using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyApplication.App.Models
{
    public class ExchangeRateFetchModel
    {
        public ExchangeRateFetchModel(Dictionary<string, string> currencyCodes, DateTime startTime, DateTime endTime)
        {
            CurrencyCodes = currencyCodes;
            StartTime = startTime;
            EndTime = endTime;

            var domainRequest = CurrencyCodes.Select(currencyCode => (Source: currencyCode.Key, Target: currencyCode.Value)).ToArray();
            SourceCurrencies = domainRequest.Select(r => r.Source).Distinct().ToArray();
            TargetCurrencies = domainRequest.Select(r => r.Target).Distinct().ToArray();
        }

        public Dictionary<string, string> CurrencyCodes { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public string[] SourceCurrencies { get; }
        public string[] TargetCurrencies { get; }
    }
}
