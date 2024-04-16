using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Models.Dto;
using src.Repository;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace src.Controllers
{
	[Authorize(Roles = "Client")]
	[Route("api/[controller]")]
	[ApiController]
	public class JobController : ControllerBase
	{
		private readonly JobRepository _jobRepo;
		private readonly CategoryRepository _categoryRepo;
		private readonly IMapper _mapper;

		public JobController(JobRepository jobRepo, CategoryRepository categoryRepo, IMapper mapper)
		{
			_jobRepo = jobRepo;
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

		// GET: api/<JobController>
		[AllowAnonymous]
		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				List<Job>? jobs = _jobRepo.GetAllWithCategoryAndClient();
				var jobsDto = _mapper.Map<List<GetJobDto>?>(jobs);
				return Ok(new Response(200, result: jobsDto));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// GET api/<JobController>/5
		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var job = _mapper.Map<GetJobDto>(_jobRepo.GetWithCategoryAndClient(id));
			if (job == null)
			{
				return NotFound(new Response(404, ["Job not found"]));
			}
			return Ok(new Response(200, job));
		}

		//POST api/<JobController>
		[HttpPost]
		public IActionResult Post([FromBody] CreateJobDto jobDto)
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Root.Errors
					.Select(e => e.ErrorMessage)
					.ToList();
				return BadRequest(new Response(400, errors));
			}
			// check Category
			var category = _categoryRepo.ReadById(jobDto.CategoryId);
			if (category == null)
			{
				return BadRequest(new Response(404, ["Category id is not valid"]));
			}
			try
			{
				var job = _mapper.Map<Job>(jobDto);
				job.ClientId = GetId();
				//ToDo disable automapping for category and assign categoryId here.
				_jobRepo.Create(job);
				return Ok(new Response(201));
			}
			catch (Exception e)
			{
				return BadRequest(new Response(400, [e.Message, e.InnerException!.Message]));
			}
		}

		// PUT api/<JobController>/5
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] CreateJobDto jobDto)
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Root.Errors
					.Select(e => e.ErrorMessage)
					.ToList();
				return BadRequest(new Response(400, errors));
			}
			// check category
			var category = _categoryRepo.ReadById(jobDto.CategoryId);
			if (category == null)
			{
				return BadRequest(new Response(404, ["Category id is not valid"]));
			}
			try
			{
				var result = _jobRepo.Update(id, jobDto);
				if (!result) 
					return NotFound(new Response(404, ["Job not found"]));
				return Ok(new Response(201));
			}
			catch (Exception e)
			{
				return BadRequest(new Response(400, [e.ToString()]));
			}
		}

		// DELETE api/<JobController>/5
		[Authorize(Roles = "Admin, Client")]
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var Job = _jobRepo.Get(id);
			if (Job == null)
			{
				return NotFound(new Response(404, ["Job not found"]));
			}
			// if client
			else if (GetRole() == "Client" && GetId() != Job.ClientId)
			{
				return Unauthorized(new Response(401, ["Not Allowed to Delete this Job"]));
			}
			else
			{
				_ = _jobRepo.Delete(id);
				return Ok(new Response(200));
			}
		}
	}
}
