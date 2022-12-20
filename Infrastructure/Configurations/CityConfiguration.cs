using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(x=> x.CityId);
            builder.Property(x => x.CityName).HasMaxLength(256);
            builder.HasMany<Township>(x => x.Townships)
                .WithOne(x => x.City)
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Cascade);            
        }
    }
}
