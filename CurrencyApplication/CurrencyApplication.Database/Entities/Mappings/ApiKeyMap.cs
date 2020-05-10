using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyApplication.Database.Entities.Mappings
{
    public class ApiKeyMap : IEntityTypeConfiguration<ApiKey>
    {
        public void Configure(EntityTypeBuilder<ApiKey> builder)
        {
            builder.HasKey(entity => entity.Value);
            builder.ToTable("api_keys");

            builder
               .Property(entity => entity.Value)
               .HasColumnName("api_key");
        }
    }
}
