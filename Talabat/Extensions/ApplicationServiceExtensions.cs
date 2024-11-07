using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entites.Identity;

namespace Talabat.APIs.Extensions
{
	public static class ApplicationServiceExtensions
	{
		public async static Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManager,ClaimsPrincipal User)
		{
			var Email = User.FindFirstValue(ClaimTypes.Email);
			var user = await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == Email);
			return user;
		}
	}
}
