using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
	public class AccountsController : APIBaseController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;

		public AccountsController(
			UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager,
			ITokenService tokenService,
			IMapper mapper
			)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenService = tokenService;
			_mapper = mapper;
		}

		// Register

		[HttpPost("Register")]
		public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
		{

			if (CheckEmailExists(model.Email).Result.Value)
			{
				return BadRequest(new APIResponse(400, "Email Already Exists"));
			}

			var User = new AppUser()
			{
				DisplayName = model.DisplayName,
				Email = model.Email,
				UserName = model.Email.Split('@')[0],
				PhoneNumber = model.PhoneNumber
			};
			var Result = await _userManager.CreateAsync(User, model.Password);
			if (!Result.Succeeded)
			{
				return BadRequest(new APIResponse(400));
			}
			var ReturnedUser = new UserDTO()
			{
				DisplayName = User.DisplayName,
				Email = User.Email,
				Token = await _tokenService.CreateTokenAsync(User, _userManager)
			};
			return Ok(ReturnedUser);
		}
		// Login
		[HttpPost("Login")]
		public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
		{
			var User = await _userManager.FindByEmailAsync(model.Email);
			if (User is null)
			{
				return Unauthorized(new APIResponse(401));
			}
			var Result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);
			if (!Result.Succeeded)
			{
				return Unauthorized(new APIResponse(401));
			}
			return Ok(new UserDTO
			{
				DisplayName = User.DisplayName,
				Email = User.Email,
				Token = await _tokenService.CreateTokenAsync(User, _userManager)
			});
		}

		// GetCurrentUser
		[Authorize]
		[HttpGet("GetCurrentUser")] // baseUrl/api/Accounts/GetCurrentUser
		public async Task<ActionResult<UserDTO>> GetCurrentUser()
		{
			var Email = User.FindFirstValue(ClaimTypes.Email);
			var user = await _userManager.FindByEmailAsync(Email);
			var ReturnedObjectDto = new UserDTO()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _tokenService.CreateTokenAsync(user, _userManager)
			};
			return Ok(ReturnedObjectDto);
		}

		[Authorize]
		[HttpGet("Address")]
		public async Task<ActionResult> GetCurrentAddress()
		{
			var user = await _userManager.FindUserWithAddressAsync(User);
			var MappedAddress = _mapper.Map<AddressDTO>(user.Address);
			return Ok(MappedAddress);
		}

		[Authorize]
		[HttpPut("Address")]
		public async Task<ActionResult<AddressDTO>> UpdateAddress(AddressDTO UpdatedAddress)
		{
			var user = await _userManager.FindUserWithAddressAsync(User);
			var MappedAddress = _mapper.Map<Address>(UpdatedAddress);
			MappedAddress.Id = user.Address.Id;
			user.Address = MappedAddress;
			var Result = await _userManager.UpdateAsync(user);
			if (!Result.Succeeded)
				return BadRequest(new APIResponse(400));
			return Ok(UpdatedAddress);
		}

		[HttpGet("emailExists")]
		public async Task<ActionResult<bool>> CheckEmailExists(string Email)
		{
			return await _userManager.FindByEmailAsync(Email) is not null;
		}
	}
}
