using Store.Magdy.Core.Dtos.Auth;
using Store.Magdy.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Magdy.Core.Services.Contract
{
    public interface IUserService
    {
        Task<UserDto> LoginAsync(LoginDto loginDto); 
        Task<UserDto> RegisterAsync(RegisterDto registrDto); 
        Task<bool> CheckEmailExistsAsync(string email);
        Task<AddressDto> UpdateAddressAsync(AppUser user, AddressDto address);
    }
}
