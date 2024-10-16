using Microsoft.AspNetCore.Identity;
using Store.Magdy.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Magdy.Repository.Identity.DataSeed
{
    public static class StoreIdentityDbContextSeed
    {
        public static async Task SeedAppUserAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    Email = "ahmed@gmail.com",
                    DisplayName = "Ahmed Amin",
                    UserName = "ahmed.amin",
                    PhoneNumber = "1234567890",
                    Address = new Address()
                    {
                        FName = "Ahmed",
                        LName = "Amin",
                        City = "ElSherouk",
                        Country = "Egypt",
                        Street = "Elshabab"
                    }
                };

                await _userManager.CreateAsync(user, "P@ssW0rd");
            }
        } 
    }
}
