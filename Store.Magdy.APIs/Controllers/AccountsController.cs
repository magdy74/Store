using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Magdy.APIs.Errors;
using Store.Magdy.APIs.Extensions;
using Store.Magdy.Core.Dtos.Auth;
using Store.Magdy.Core.Entities.Identity;
using Store.Magdy.Core.Services.Contract;
using Store.Magdy.Service.Services.Tokens;
using System.Security.Claims;

namespace Store.Magdy.APIs.Controllers
{
    public class AccountsController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(IUserService userService, UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            _userService = userService;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("login")] // Post : /api/accounts/login
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userService.LoginAsync(loginDto);
            
            if (user == null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            
            return Ok(user);
        }

        [HttpPost("register")] // Post : /api/accounts/login
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var user = await _userService.RegisterAsync(registerDto);

            if (user == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid Registration"));

            return Ok(user);
        }

        [Authorize]
        [HttpGet("GetCurrentUser")]  // GET: /api/Accounts/GetCurrentUser
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.createTokenAsync(user, _userManager)

            });
        }

        [Authorize]
        [HttpGet("Address")]  // GET: /api/Accounts/Address
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var user = await _userManager.FindByEmailWithAddress(User);

            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<AddressDto>(user.Address));
        }

        [Authorize]
        [HttpPost("Address")]  // GET: /api/Accounts/Address
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto address)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var user = await _userManager.FindByEmailWithAddress(User);

            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var UpdatedAddress = await _userService.UpdateAddressAsync(user, address);

            if(UpdatedAddress is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(UpdatedAddress);
        }

    }
}
