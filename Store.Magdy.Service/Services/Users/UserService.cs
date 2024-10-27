using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Store.Magdy.Core.Dtos.Auth;
using Store.Magdy.Core.Entities.Identity;
using Store.Magdy.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Magdy.Service.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null) return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return null;

            return new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.createTokenAsync(user, _userManager)

            };

        }

        public async Task<UserDto> RegisterAsync(RegisterDto registrDto)
        {
            if (await CheckEmailExistsAsync(registrDto.Email)) return null;

            var user = new AppUser()
            {
                Email = registrDto.Email,
                DisplayName = registrDto.DisplayName,
                UserName = registrDto.Email.Split("@")[0],
            };

            var result = await _userManager.CreateAsync(user, registrDto.Password);

            if (!result.Succeeded) return null;

            return new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.createTokenAsync(user, _userManager)
            };

        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

        public async Task<AddressDto> UpdateAddressAsync(AppUser user, AddressDto address)
        {
            user.Address = _mapper.Map<Address>(address);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return null;

            return address;
        }

    }
}
