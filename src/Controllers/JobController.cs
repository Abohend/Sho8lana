using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sho8lana.API.Exceptions;
using Sho8lana.DataAccess.Repositories;
using Sho8lana.Entities.Models;
using Sho8lana.Entities.Models.Dto.Job;
using System.Security.Claims;

namespace Sho8lana.API.Controllers
{
	[Authorize(Roles = "Freelancer")]
	[Route("api/[controller]")]
	[ApiController]
	public class JobController : ControllerBase
	{
		private readonly JobRepository _jobRepo;
		private readonly ProjectRepository _projectRepo;

		#region Helpers
		private string GetId()
		{
			return User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
		}
		#endregion

		public JobController(JobRepository jobRepository, ProjectRepository projectRepository)
		{
			this._jobRepo = jobRepository;
			this._projectRepo = projectRepository;
		}

		// GET: api/<JobController>
		// returns all the jobs for the project that freelancer take
		[HttpGet("{projectId:int}")]
		public IActionResult Get(int projectId)
		{
			// make sure the authorized user is the owner of the project
			var project = _projectRepo.Read(projectId)
				?? throw new NotFoundException("Project Id not found"); 

			if (_projectRepo.ReadProjectTakerId(projectId) != GetId())
                throw new UnauthorizedAccessException();

            var jobs = _jobRepo.ReadAll(projectId);
			return Ok(new Response(200, jobs));
		}


		// GET api/<JobController>/5
		// returns all the suggested jobs to a specific freelancer
		[HttpGet("{freelancerId}")]
		public IActionResult Get(string freelancerId)
		{
			// make sure that freelancer is getting his own data
			if (freelancerId != GetId())
			{
				throw new UnauthorizedAccessException("Invalid freelancer Id");
			}
			var jobs = _jobRepo.ReadAll(freelancerId);
			return Ok(new Response(200, jobs));
		}


		// POST api/<JobController>
		[HttpPost("{projectId}")]
		public IActionResult Post(int projectId, [FromBody] List<CreateJobDto> JobsDto)
		{
			var project = _projectRepo.Read(projectId)
                ?? throw new NotFoundException("Project Id not found");

			// make sure the authorized user is the owner of the project
            if (_projectRepo.ReadProjectTakerId(projectId) != GetId())
                throw new UnauthorizedAccessException();

            _jobRepo.Create(projectId, JobsDto);
			return Ok(new Response(201, JobsDto));
		}

		// DELETE api/<JobController>/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			// only allowed for freelancer who created it and must sure that no freelancer is assigned to it
			var job = _jobRepo.Read(id);
			if (_projectRepo.ReadProjectTakerId(job.ProjectId) != GetId())
				throw new UnauthorizedAccessException("Invalid Job Id");

			else if (_jobRepo.ReadJobTakerId(id) != null)
                throw new UnauthorizedAccessException();


            var result = _jobRepo.Delete(id);
			if (result)
			{
				return Ok(new Response(200));
			}
			throw new BadRequestException("Something went wrong!");
		}
	
	}
}
