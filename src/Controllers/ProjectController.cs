using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Models.Dto.Project;
using src.Repository;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace src.Controllers
{
    [Authorize(Roles = "Client")]
	[Route("api/[controller]")]
	[ApiController]
	public class ProjectController : ControllerBase
	{
		private readonly ProjectRepository _projectRepo;
		private readonly CategoryRepository _categoryRepo;
		private readonly IMapper _mapper;

		public ProjectController(ProjectRepository projectRepo, CategoryRepository categoryRepo, IMapper mapper)
		{
			_projectRepo = projectRepo;
			_categoryRepo = categoryRepo;
			_mapper = mapper;
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
				List<Project>? projects = _projectRepo.GetAllWithCategoryAndClient();
				var projectsDto = _mapper.Map<List<GetProjectDto>?>(projects);
				return Ok(new Response(200, result: projectsDto));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// GET api/<ProjectController>/5
		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var project = _mapper.Map<GetProjectDto>(_projectRepo.GetWithCategoryAndClient(id));
			if (project == null)
			{
				return NotFound(new Response(404, ["Project not found"]));
			}
			return Ok(new Response(200, project));
		}

		//POST api/<ProjectController>
		[HttpPost]
		public IActionResult Post([FromBody] CreateProjectDto projectDto)
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Root.Errors
					.Select(e => e.ErrorMessage)
					.ToList();
				return BadRequest(new Response(400, errors));
			}
			// check Category
			var category = _categoryRepo.Get(projectDto.CategoryId);
			if (category == null)
			{
				return BadRequest(new Response(404, ["Category id is not valid"]));
			}
			try
			{
				var project = _mapper.Map<Project>(projectDto);
				project.ClientId = GetId();
				//ToDo disable automapping for category and assign categoryId here.
				_projectRepo.Create(project);
				return Ok(new Response(201));
			}
			catch (Exception e)
			{
				return BadRequest(new Response(400, [e.Message, e.InnerException!.Message]));
			}
		}

		// PUT api/<ProjectController>/5
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] CreateProjectDto projectDto)
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Root.Errors
					.Select(e => e.ErrorMessage)
					.ToList();
				return BadRequest(new Response(400, errors));
			}
			// check category
			var category = _categoryRepo.Get(projectDto.CategoryId);
			if (category == null)
			{
				return BadRequest(new Response(404, ["Category id is not valid"]));
			}
			try
			{
				var result = _projectRepo.Update(id, projectDto);
				if (!result) 
					return NotFound(new Response(404, ["Project not found"]));
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
			var project = _projectRepo.Get(id);
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
