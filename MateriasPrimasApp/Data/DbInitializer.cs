using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MateriasPrimasApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(UserManager<IdentityUser> userManger, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return; // DB has been seeded
            }

            string password = "Admin123*";
            string roleName = "Administrador";
            IdentityUser user = new IdentityUser() { UserName = "Admin", Email = "admin@test.com" };
            userManger.CreateAsync(user, password);
            IdentityRole roll = new IdentityRole() { Name = roleName };
            roleManager.CreateAsync(roll);
            userManger.AddToRoleAsync(user, roleName);
        }
    }
}
