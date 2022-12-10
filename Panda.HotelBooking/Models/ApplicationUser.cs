using Microsoft.AspNetCore.Identity;

namespace Panda.HotelBooking.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
    }
    public class ApplicationRole : IdentityRole<Guid>
    {

    }
}
