using Microsoft.AspNetCore.Identity;
using Store.Magdy.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Magdy.Core.Services.Contract
{
    public interface ITokenService
    {
        Task<string> createTokenAsync(AppUser user, UserManager<AppUser> userManager);
    }
}
