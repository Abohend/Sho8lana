using AutoMapper;
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
		private readonly AccountRepository _accountRepo;
		private readonly IMapper _mapper;
		private readonly CategoryRepository _categoryRepo;

		public AccountController(AccountRepository repo, IMapper mapper, CategoryRepository categoryRepository)
        {
			this._accountRepo = repo;
			this._mapper = mapper;
			this._categoryRepo = categoryRepository;
		}

		[HttpPost("register/client")]
		public async Task<IActionResult> Register(RegisterClientDto clientDto)
		{
			// check email uniqueness
			bool uniqueResult = await _accountRepo.UniqueEmail(clientDto.Email);
			if (!uniqueResult)
			{
				return Conflict(new Response(StatusCodes.Status400BadRequest, ["Email Already Taken"]));
			}

			var result = await _accountRepo.RegisterClientAsync(_mapper.Map<Client>(clientDto), clientDto.Password);

			if (!result.Succeeded)
			{
				List<string> errors = result.Errors
					.Select(e => e.Description)
					.ToList();
				return BadRequest(new Response(StatusCodes.Status400BadRequest, errors));
			}
			else
			{
				return Ok(new Response(StatusCodes.Status201Created));
			}
		}

		[HttpPost("register/freelancer")]
		public async Task<IActionResult> Register(RegisterFreelancerDto freelancerDto)
		{
			// check email uniqueness
			bool uniqueResult = await _accountRepo.UniqueEmail(freelancerDto.Email);
			if (!uniqueResult)
			{
				return Conflict(new Response(StatusCodes.Status400BadRequest, ["Email Already Taken"]));
			}

			// check category existance
			var category = _categoryRepo.Get(freelancerDto.CategoryId);
			if (category == null)
			{
				return BadRequest(new Response(StatusCodes.Status404NotFound, ["Category not found"]));
			}

			var result = await _accountRepo.RegisterFreelancerAsync(_mapper.Map<Freelancer>(freelancerDto), freelancerDto.Password);

			if (!result.Succeeded)
			{
				List<string> errors = result.Errors
					.Select(e => e.Description)
					.ToList();
				return BadRequest(new Response(StatusCodes.Status400BadRequest, errors));
			}
			else
			{
				return Ok(new Response(StatusCodes.Status201Created));
			}
		}

		[HttpPost("login")]
		public async Task<IActionResult> SignIn(UserSigninDto user)
		{
			try
			{
				var loginResponse = await _accountRepo.SiginInAsync(user);
				if (loginResponse == null)
				{
					return Unauthorized(new Response(StatusCodes.Status401Unauthorized, ["Sign In Credentials not valid"]));
				}
				return Ok(new Response(StatusCodes.Status202Accepted, loginResponse));
			}
			catch
			{
				return Unauthorized(new Response(StatusCodes.Status401Unauthorized, ["Unexpected Error!"]));
			}

		}
	
	}
}
