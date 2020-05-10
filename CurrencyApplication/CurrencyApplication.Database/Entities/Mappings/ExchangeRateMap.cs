using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyApplication.Database.Entities.Mappings
{
    public class ExchangeRateMap : IEntityTypeConfiguration<ExchangeRate>
    {
        public void Configure(EntityTypeBuilder<ExchangeRate> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.ToTable("exchange_rates");

            builder
               .Property(entity => entity.Id)
               .HasColumnName("exchange_rate_id");

            builder
                .Property(entity => entity.Date)
                .HasColumnName("date");

            builder
                .Property(entity => entity.Source)
                .HasColumnName("source_currency");

            builder
                .Property(entity => entity.Target)
                .HasColumnName("target_currency");

            builder
                .Property(entity => entity.Value)
                .HasColumnName("value");
        }
    }
}
