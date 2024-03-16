using Microsoft.AspNetCore.Http;
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
		private Response _responce;
		public AccountController(AccountRepository repo, Response responce)
        {
			this._repo = repo;
			_responce = responce;
		}
        
		[HttpPost("/register")]
		public async Task<IActionResult> Register(UserRegisterDto user)
		{
			// fault entry
			if (!ModelState.IsValid) 
			{
				var errors = ModelState.Root.Errors
					.Select(e => e.ErrorMessage)
					.ToList();
				return BadRequest(new Response(400, false, errors));
			}
			else
			{
				try
				{
					bool uniqueResult = await _repo.IsUniqueEmailAsync(user.Email);
					if (!uniqueResult)
					{
						return Conflict(new Response(StatusCodes.Status400BadRequest, false, ["Email Already Taken"]));
					}
				}
				catch (Exception ex)
				{
					return Conflict(new Response(StatusCodes.Status400BadRequest, false, [ex.Message]));
				}
				
				try
				{
					bool result = await _repo.RegisterAsync(user);
					if (!result)
					{
						_responce = new Response(StatusCodes.Status400BadRequest, false, ["Email Already Taken"]);
					}
				}
				catch (Exception ex)
				{
					_responce = new Response(StatusCodes.Status400BadRequest, false, [ex.Message]);
				}
			
				return Ok(new Response(StatusCodes.Status201Created));
			}
		}
	}
}
