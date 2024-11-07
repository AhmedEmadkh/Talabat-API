using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Identity;

namespace Talabat.Repository.Identity
{
	public static class AppIdentityDbContextSeed
	{
		public static async Task SeedUserAsyn(UserManager<AppUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var User = new AppUser()
				{
					DisplayName = "Ahmed",
					Email = "ae3866948@gmail.com",
					UserName = "Eng.Ahmed",
					PhoneNumber = "01028618665"
				};
				await userManager.CreateAsync(User, "P@ssw0rd");
			}
		}
	}
}
