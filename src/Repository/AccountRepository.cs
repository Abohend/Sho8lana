using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using src.Data;
using src.Models;
using src.Models.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace src.Repository
{
	public class AccountRepository
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IConfiguration _config;
		private readonly IMapper _mapper;

		public AccountRepository(UserManager<ApplicationUser> userManager, IConfiguration config, IMapper mapper)
        {
			_userManager = userManager;
			this._config = config;
			_mapper = mapper;
		}

		public async Task<bool> UniqueEmail(string email)
		{
			var exitedUser = await _userManager.FindByEmailAsync(email);
			if (exitedUser != null)
			{
				return false;
			}
			return true;
		}

		public async Task<IdentityResult> RegisterClientAsync(Client client, string password)
		{
			var result = await _userManager.CreateAsync(client, password);
			if (result.Succeeded)
			{
				result = await _userManager.AddToRoleAsync(client, "client");
			}
			return result;
		}

		public async Task<IdentityResult> RegisterFreelancerAsync(Freelancer freelancer, string password)
		{
			var result = await _userManager.CreateAsync(freelancer, password);
			if (result.Succeeded)
			{
				result = await _userManager.AddToRoleAsync(freelancer, "freelancer");
			}
			return result;
		}
    
		public async Task<string> SiginInAsync(UserSigninDto userDto)
		{
			var user = await _userManager.FindByEmailAsync(userDto.Email);
			if (user != null)
			{
				var result = await _userManager.CheckPasswordAsync(user, userDto.Password);
				if (result)
				{
					var roles = await _userManager.GetRolesAsync(user);
					var role = roles.FirstOrDefault();
					var claims = new[]
					{
						new Claim(ClaimTypes.NameIdentifier, user.Id),
						new Claim(ClaimTypes.Role, role!)
					};
					var token = new JwtSecurityToken
					(
						issuer: _config["JWT:Issuer"],
						audience: _config["JWT:Audience"],
						signingCredentials: new SigningCredentials(
							new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!)),
							SecurityAlgorithms.HmacSha256Signature),
						expires: DateTime.Now.AddHours(8),
						claims: claims
					);
					var serliazedToken = new JwtSecurityTokenHandler().WriteToken(token);
					return serliazedToken;
				}
			}
			return String.Empty;
		}
	}
}
