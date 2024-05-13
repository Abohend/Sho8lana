using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Models.Dto.Client;
using src.Models.Dto.Freelancer;
using src.Repository;
using src.Services;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace src.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FreelancerController : ControllerBase
	{
		private readonly FreelancerRepository _freelancerRepo;
		private readonly SkillRepository _skillRepo;
		private readonly CategoryRepository _categoryRepo;

		public FreelancerController(FreelancerRepository freelancerRepository
			, SkillRepository skillRepository
			, CategoryRepository categoryRepository)
		{
			this._freelancerRepo = freelancerRepository;
			this._skillRepo = skillRepository;
			this._categoryRepo = categoryRepository;
		}

		#region Helpers
		private string? GetImageUrl(string? path)
		{
			if (path != null)
				return $"{Request.Scheme}://{Request.Host}/{path}";
			return null;
		}
		private string GetId()
		{
			return User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
		}
		private string GetRole()
		{
			return User.FindFirst(ClaimTypes.Role)!.Value;
		}
		#endregion

		// GET: api/<FreelancerController>
		[HttpGet]
		public IActionResult Get()
		{
			var freelancers = _freelancerRepo.ReadAllWithSkills();

			// update image pathes
			foreach (var freelancer in freelancers!)
			{
				freelancer.ImageUrl = GetImageUrl(freelancer.ImageUrl);
			}

			return Ok(new Response(200, freelancers));
		}

		// GET api/<FreelancerController>/5
		[HttpGet("{id}")]
		public IActionResult Get(string id)
		{
			var freelancer = _freelancerRepo.ReadWithSkills(id);
			if (freelancer != null)
			{
				freelancer.ImageUrl = GetImageUrl(freelancer.ImageUrl);
				return Ok(new Response(200, freelancer));
			}
			return BadRequest(new Response(404, ["Freelancer not found"]));
		}

		// PUT api/<FreelancerController>/5
		[HttpPut("{id}")]
		public IActionResult Put(string id, [FromForm] UpdateFreelancerDto freelancerDto)
		{
			// Authorized Freelancer
			if (GetId() != id)
			{
				return Unauthorized(new Response(StatusCodes.Status203NonAuthoritative, ["Invalid Freelancer Id"]));
			}

			// Category Id existence
			var categoryId = freelancerDto.CategoryId;
			if (categoryId != null)
			{
				var category = _categoryRepo.Get(categoryId.Value);
				if (category == null)
				{
					return BadRequest(new Response(StatusCodes.Status406NotAcceptable, ["Invalid Category Id"]));
				}
			}

			// Skills existence
			var skillsId = freelancerDto.SkillsId;
			List<Skill>? newSkills = null;
			if (skillsId != null)
			{
				newSkills = new();
				foreach (var skillId in skillsId)
				{
					var skill = _skillRepo.ReadById(skillId);
					if (skill == null)
						return BadRequest(new Response(404, "Enter a valid Skill Ids"));
					newSkills.Add(skill);
				}
			}

			// Try Update
			_freelancerRepo.Update(id, freelancerDto, newSkills);
			return Ok(new Response(200));
		}

		// DELETE api/<FreelancerController>/5
		[HttpDelete("{id}")]
		public IActionResult Delete(string id)
		{
			if (GetId() == id || GetRole() == "Admin")
			{
				_ = _freelancerRepo.Delete(id);
				return Ok(new Response(200));
			}
			else
			{
				return Unauthorized(new Response(StatusCodes.Status203NonAuthoritative, ["Freelancer Id not valid"]));
			}
		}
	}
}
