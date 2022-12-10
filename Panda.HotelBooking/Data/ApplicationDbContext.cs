using Microsoft.EntityFrameworkCore;
using Panda.HotelBooking.Models;
namespace Panda.HotelBooking.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Panda.HotelBooking.Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<Township> Townships { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<BedType> BedTypes { get; set; }    
        public DbSet<FacilityType> FacilityTypes { get; set; }

        public DbSet<Room> Rooms { get; set; }  
        public DbSet<RoomBed> RoomBeds { get; set; }
        public DbSet<RoomPhoto> RoomPhotos { get; set; }    
        public DbSet<RoomFacility> RoomFacilities { get; set; }
        public DbSet<Panda.HotelBooking.Models.ApplicationRole> ApplicationRole { get; set; }
    }
}