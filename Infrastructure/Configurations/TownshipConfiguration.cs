using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class TownshipConfiguration : IEntityTypeConfiguration<Township>
    {
        public void Configure(EntityTypeBuilder<Township> builder)
        {
            builder.HasKey(x => x.TownshipId);
            builder.HasOne<City>(x => x.City).WithMany(x => x.Townships).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany<Hotel>(x => x.Hotels).WithOne(x => x.Township).HasForeignKey(x => x.TownshipId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
