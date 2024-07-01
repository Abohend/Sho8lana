using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Models.Dto.Project;
using src.Repository;
using System.Security.Claims;


namespace src.Controllers
{
    //[Authorize(Roles = "Client")]
	[Route("api/[controller]")]
	[ApiController]
	public class ProjectController : ControllerBase
	{
		private readonly ProjectRepository _projectRepo;
		private readonly CategoryRepository _categoryRepo;
		private readonly SkillRepository _skillRepo;

		public ProjectController(ProjectRepository projectRepo, 
			CategoryRepository categoryRepo,
			SkillRepository skillRepository)
		{
			_projectRepo = projectRepo;
			_categoryRepo = categoryRepo;
			this._skillRepo = skillRepository;
		}

		#region Helpers
		private string GetRole()
		{
			return User.FindFirst(ClaimTypes.Role)!.Value.ToLower();
		}
		private string GetId()
		{
			return User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
		}
		#endregion


		// GET: api/<ProjectController>
		[AllowAnonymous]
		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				var projects = _projectRepo.ReadAll();
				return Ok(new Response(200, result: projects));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// GET api/<ProjectController>/5
		[AllowAnonymous]
		[HttpGet("{id:int}")]
		public IActionResult Get(int id)
		{
			var project = _projectRepo.ReadWithSkills(id);
			if (project == null)
			{
				return NotFound(new Response(404, ["Project not found"]));
			}
			return Ok(new Response(200, project));
		}

        [Authorize(Roles = "Freelancer")]
        [HttpGet("freelancer/{freelancerId}")]
		// return all project for a freelancer
		public IActionResult Get(string freelancerId)
		{
			if (GetId() != freelancerId)
			{
				return BadRequest(new Response(401, ["You are not allowed to view projects of other freelancers"]));
			}
			var projects = _projectRepo.ReadAll(freelancerId);
			return Ok(new Response(200, projects));
		}

        //POST api/<ProjectController>
        [Authorize(Roles = "Client")]
        [HttpPost]
		public IActionResult Post([FromBody] CreateProjectDto projectDto)
		{
			// check Category
			var category = _categoryRepo.Get(projectDto.CategoryId);
			if (category == null)
			{
				return BadRequest(new Response(404, ["Category id is not valid"]));
			}

			List<Skill>? skills = null;

			if (projectDto.RequiredSkillsId != null)
			{
				skills = new List<Skill>();
				foreach (var skillId in projectDto.RequiredSkillsId)
				{
					var skill = _skillRepo.ReadById(skillId);
					if (skill == null)
					{
						return BadRequest(new Response(404, ["Enter valid skills"]));
					}
					else
					{
						skills.Add(skill);
					}
				}
			}

			//ToDo disable automapping for category and assign categoryId here.
			_projectRepo.Create(GetId(), skills, projectDto);
			return Ok(new Response(201));
		}


        // PUT api/<ProjectController>/5
        [Authorize(Roles = "Client")]
        [HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] CreateProjectDto projectDto)
		{
			// check category existence
			var category = _categoryRepo.Get(projectDto.CategoryId);
			if (category == null)
			{
				return BadRequest(new Response(404, ["Category id is not valid"]));
			}
			try
			{
				var project = _projectRepo.Read(id);
				if (project == null)
					return NotFound(new Response(404, ["Project not found"]));

				// check the updator
				if (GetRole() == "Client" && GetId() != project.ClientId)
				{
					return Unauthorized(new Response(401, ["Not Allowed to Update this Project"]));
				}

				// validating skills id
				foreach (var skillId in projectDto.RequiredSkillsId!)
				{
					var skill = _skillRepo.ReadById(skillId);
					if (skill == null)
					{
						return BadRequest(new Response(404, ["Enter valid skills"]));
					}
				}

				var result = _projectRepo.Update(id, projectDto);

				return Ok(new Response(201));
			}
			catch (Exception e)
			{
				return BadRequest(new Response(400, [e.ToString()]));
			}
		}


		// DELETE api/<ProjectController>/5
		[Authorize(Roles = "Admin, Client")]
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var project = _projectRepo.Read(id);
			if (project == null)
			{
				return NotFound(new Response(404, ["Project not found"]));
			}
			// if client
			else if (GetRole() == "Client" && GetId() != project.ClientId)
			{
				return Unauthorized(new Response(401, ["Not Allowed to Delete this Project"]));
			}
			else
			{
				_ = _projectRepo.Delete(id);
				return Ok(new Response(200));
			}
		}
	}
}
