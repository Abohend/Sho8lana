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

		public async Task<IdentityResult> RegisterAsync(UserRegisterDto userDto)
		{
			//todo: use Mapping
			IdentityResult result;
			if (userDto.AccountType == AccountType.Freelancer)
			{
				var user = new Freelancer
				{
					Email = userDto.Email,
					UserName = userDto.Email,
					Name = userDto.Name
				};
				result = await _userManager.CreateAsync(user, userDto.Password);
				if (result.Succeeded)
				{
					result = await _userManager.AddToRoleAsync(user, AccountType.Freelancer.ToString());
				}
			}
			else 
			{
				var user = new ApplicationUser
				{
					Email = userDto.Email,
					UserName = userDto.Email,
					Name = userDto.Name
				};
				result = await _userManager.CreateAsync(user, userDto.Password);
				if (result.Succeeded)
				{
					result = await _userManager.AddToRoleAsync(user, AccountType.Client.ToString());
				}
			}
			
			return result;
		}
    }
}
