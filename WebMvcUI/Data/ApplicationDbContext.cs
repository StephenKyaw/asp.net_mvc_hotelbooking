using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebMvcUI.Data
{
    public class ApplicationDbContextV2 : IdentityDbContext
    {
        public ApplicationDbContextV2(DbContextOptions<ApplicationDbContextV2> options)
            : base(options)
        {
        }
    }
}