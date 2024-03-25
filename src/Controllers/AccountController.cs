using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Models.Dto;
using src.Repository;

namespace src.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly AccountRepository _repo;
		public AccountController(AccountRepository repo)
        {
			this._repo = repo;
		}
        
		[HttpPost("register")]
		public async Task<IActionResult> Register(UserRegisterDto user)
		{
			// parsing type
			if (!Enum.TryParse(user.AccountType.ToLower(), out AccountType accountType))
			{
				return BadRequest(new Response(400, ["Account Type not valid"]));
			}
			// fault entry
			if (!ModelState.IsValid) 
			{
				var errors = ModelState.Root.Errors
					.Select(e => e.ErrorMessage)
					.ToList();
				return BadRequest(new Response(400, errors));
			}
			else
			{
				// check unique email
				try
				{
					bool uniqueResult = await _repo.UniqueEmail(user.Email);
					if (!uniqueResult)
					{
						return Conflict(new Response(StatusCodes.Status400BadRequest, ["Email Already Taken"]));
					}
				}
				catch (Exception ex)
				{
					return Conflict(new Response(StatusCodes.Status400BadRequest, [ex.Message]));
				}
				
				// check registeration process
				try
				{
					var result = await _repo.RegisterAsync(user);
					if (!result.Succeeded)
					{
						List<string> errors = result.Errors
							.Select(e => e.Description)
							.ToList();
						return BadRequest(new Response(StatusCodes.Status400BadRequest, errors));
					}
					return Ok(new Response(StatusCodes.Status201Created));
				}
				catch (Exception ex)
				{
					return BadRequest(new Response(StatusCodes.Status400BadRequest, [ex.Message]));
				}
			
				
			}
		}

		[HttpPost("login")]
		public async Task<IActionResult> SignIn(UserSigninDto user)
		{
			if (!ModelState.IsValid)
			{
				List<string> errors = ModelState.Root.Errors
					.Select(e => e.ErrorMessage)
					.ToList();
				return BadRequest(new Response(StatusCodes.Status406NotAcceptable, errors));
			}
			try
			{
				string token = await _repo.SiginInAsync(user);
				if (token == String.Empty)
				{
					return Unauthorized(new Response(StatusCodes.Status401Unauthorized, ["Sign In Credentials not valid"]));
				}
				return Ok(new Response(StatusCodes.Status202Accepted, token));
			}
			catch
			{
				return Unauthorized(new Response(StatusCodes.Status401Unauthorized, ["Unexpected Error!"]));
			}

		}
	
	}
}
