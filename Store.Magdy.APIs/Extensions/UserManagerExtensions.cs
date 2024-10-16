using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Magdy.APIs.Errors;
using Store.Magdy.Core.Entities.Identity;
using System.Security.Claims;

namespace Store.Magdy.APIs.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindByEmailWithAddress(this UserManager<AppUser> userManager, ClaimsPrincipal User)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null) return null;

            var user = await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == userEmail);

            if (user is null) return null;

            return user;
        }
    }
}
