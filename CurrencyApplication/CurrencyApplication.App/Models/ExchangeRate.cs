using System;

namespace CurrencyApplication.App.Models
{
    public class ExchangeRate
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
    }
}
