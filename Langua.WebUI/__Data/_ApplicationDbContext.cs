using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Langua.WebUI.__Data
{
    public class _ApplicationDbContext(DbContextOptions<_ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
    }
}
