using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasKey(x => x.HotelId);

            builder.HasOne<City>(c => c.City).WithMany(x => x.Hotels).HasForeignKey(x => x.CityId);

            builder.HasOne<Township>(t => t.Township).WithMany(x => x.Hotels).HasForeignKey(x => x.TownshipId);

            builder.HasMany<Room>(x => x.Rooms).WithOne(x => x.Hotel).HasForeignKey(x => x.HotelId).OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class HotelPhotoConfiguration : IEntityTypeConfiguration<HotelPhoto>
    {
        public void Configure(EntityTypeBuilder<HotelPhoto> builder)
        {
            builder.HasKey(x => x.HotelPhotoId);

            builder.HasOne<Hotel>(x => x.Hotel).WithMany(x => x.HotelPhotos).HasForeignKey(x => x.HotelId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
