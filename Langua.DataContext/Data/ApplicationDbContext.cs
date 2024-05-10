using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Langua.Models;
using System.Reflection.Metadata;

namespace Langua.DataContext.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users");
            //builder.Entity<IdentityRole>().ToTable("Roles").HasData(new IdentityRole
            //{
            //    Name = "ADMIN",
            //    NormalizedName = "Admin"
            //},
            //        new IdentityRole
            //        {
            //            Name = "TEACHER",
            //            NormalizedName = "Teacher"
            //        },
            //        new IdentityRole
            //        {
            //            Name = "MANAGER",
            //            NormalizedName = "Manager"
            //        });
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("UserRoleClaim");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");


        }
    }
}
