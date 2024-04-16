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
		private readonly CategoryRepository _categoryRepo;

		public AccountRepository(UserManager<ApplicationUser> userManager
			, IConfiguration config
			, IMapper mapper
			, CategoryRepository categoryRepo)
        {
			_userManager = userManager;
			this._config = config;
			_mapper = mapper;
			this._categoryRepo = categoryRepo;
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

		public async Task<IdentityResult> RegisterAsync(UserRegisterDto userDto)
		{
			IdentityResult result;
			if (userDto.AccountType == AccountType.freelancer.ToString())
			{
				Freelancer user = _mapper.Map<Freelancer>(userDto);
				result = await _userManager.CreateAsync(user, userDto.Password);
				if (result.Succeeded)
				{
					result = await _userManager.AddToRoleAsync(user, AccountType.freelancer.ToString());
				}
			}
			else 
			{
				Client user = _mapper.Map<Client>(userDto);
				result = await _userManager.CreateAsync(user, userDto.Password);
				if (result.Succeeded)
				{
					result = await _userManager.AddToRoleAsync(user, AccountType.client.ToString());
				}
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
