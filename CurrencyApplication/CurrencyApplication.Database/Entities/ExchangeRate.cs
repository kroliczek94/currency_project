using System;

namespace CurrencyApplication.Database.Entities
{
    public class ExchangeRate
    {
        public long Id { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}
