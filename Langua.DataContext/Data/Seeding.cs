using Langua.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.DataContext.Data
{
    public static class Seeding
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var userM = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleM = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


                List<string> Roles = new List<string>() { "ADMIN", "TEACHER", "MANAGER", "MANAGERETAB","CANDIDATE" };

                foreach (var role in Roles)
                {
                    if (await roleM.RoleExistsAsync(role) != true) {
                        var resultRole = await roleM.CreateAsync(new IdentityRole(role));
                    }
                }
                ApplicationUser _user = new ApplicationUser()
                {
                    FullName = "Admin",
                    Email = "Hamzamouddakur@gmail.com",
                    UserName = "Hamzamouddakur@gmail.com"
                };
                var ExistUser = await userM.FindByEmailAsync(_user.Email);
                if (ExistUser is null)
                {

                    var user = await userM.CreateAsync(_user, "Hamza=Langua123");
                    if (user.Succeeded)
                        await userM.AddToRoleAsync(_user, "ADMIN");
                }
            }
            
        }
    }
}
