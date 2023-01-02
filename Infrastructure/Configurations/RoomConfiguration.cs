using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(x => x.RoomId);

            builder.Property(b => b.Price).HasPrecision(18, 2);

            builder.HasOne<Hotel>(x => x.Hotel).WithMany(x => x.Rooms).HasForeignKey(x => x.HotelId);
            builder.HasOne<RoomType>(x => x.RoomType).WithMany(x => x.Rooms).HasForeignKey(x => x.RoomTypeId);



            builder.HasMany<RoomBed>(x => x.RoomBeds).WithOne(x => x.Room).HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany<RoomFacility>(x => x.RoomFacilities).WithOne(x => x.Room).HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany<RoomPhoto>(x => x.RoomPhotos).WithOne(x => x.Room).HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class RoomBedConfiguration : IEntityTypeConfiguration<RoomBed>
    {
        public void Configure(EntityTypeBuilder<RoomBed> builder)
        {
            builder.HasKey(x => x.RoomBedId);

            builder.HasOne<BedType>(x => x.BedType).WithMany(x => x.RoomBeds).HasForeignKey(x => x.BedTypeId);
        }
    }

    public class RoomFacilityConfiguration : IEntityTypeConfiguration<RoomFacility>
    {
        public void Configure(EntityTypeBuilder<RoomFacility> builder)
        {
            builder.HasKey(x => x.RoomFacilityId);

            builder.HasOne<FacilityType>(x => x.FacilityType).WithMany(x => x.RoomFacilities).HasForeignKey(x => x.FacilityTypeId);
        }
    }

    public class RoomPhotoConfiguration : IEntityTypeConfiguration<RoomPhoto>
    {
        public void Configure(EntityTypeBuilder<RoomPhoto> builder)
        {
            builder.HasKey(x => x.RoomPhotoId);
        }
    }

    public class BedTypeConfiguration : IEntityTypeConfiguration<BedType>
    {
        public void Configure(EntityTypeBuilder<BedType> builder)
        {
            builder.HasKey(x => x.BedTypeId);
        }
    }

    public class FacilityTypeConfiguration : IEntityTypeConfiguration<FacilityType>
    {
        public void Configure(EntityTypeBuilder<FacilityType> builder)
        {
            builder.HasKey(x => x.FacilityTypeId);
        }
    }

    public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
    {
        public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            builder.HasKey(x => x.RoomTypeId);
            builder.HasMany<Room>(x => x.Rooms).WithOne(x => x.RoomType).HasForeignKey(x => x.RoomTypeId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
