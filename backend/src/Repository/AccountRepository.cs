using Microsoft.AspNetCore.Identity;
using src.Data;
using src.Models;
using src.Models.Dto;

namespace src.Repository
{
	public class AccountRepository
	{
		private readonly UserManager<ApplicationUser> _userManager;
		public AccountRepository(UserManager<ApplicationUser> userManager)
        {
			_userManager = userManager;
		}

		public async Task<bool> IsUniqueEmailAsync(string email)
		{
			var exitedUser = await _userManager.FindByEmailAsync(email);
			if (exitedUser != null)
			{
				return false;
			}
			return true;
		}

		public async Task<bool> RegisterAsync(UserRegisterDto userDto)
		{
			//todo: use Mapping
			var user = new ApplicationUser{
				Email = userDto.Email,
				UserName = userDto.Email,
				Name = userDto.Name
			};
			
			if (userDto.AccountType == AccountType.Freelancer)
			{
				var freelancer = user as Freelancer;
				await _userManager.CreateAsync(user, userDto.Password);
				await _userManager.AddToRoleAsync(user, AccountType.Freelancer.ToString());
			}
			else 
			{
				var client = user as Client;
				await _userManager.CreateAsync(user, userDto.Password);
				await _userManager.AddToRoleAsync(user, AccountType.Client.ToString());
			}
			return true;
		}
    }
}
