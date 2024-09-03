using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sho8lana.DataAccess.Repositories;
using System.Security.Claims;
using Sho8lana.Entities.Models;
using Sho8lana.Entities.Models.Dto.Proposal;

namespace Sho8lana.API.Controllers
{
	[Authorize(Roles = "Freelancer")]
	[Route("api/[controller]")]
	[ApiController]
	public class JobProposalController : ControllerBase
	{
		private readonly JobProposalRepository _jobProposalRepo;
		private readonly JobRepository _jobRepo;
		private readonly ProjectRepository _projectRepo;
		private readonly FreelancerRepository _freelancerRepo;

		#region Helpers
		private string GetId()
		{
			return User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
		}
		#endregion

		public JobProposalController(JobProposalRepository JobProposalRepository,
			JobRepository JobRepository,
			ProjectRepository _projectRepo,
			FreelancerRepository _freelancerRepo)
		{
			this._jobProposalRepo = JobProposalRepository;
			this._jobRepo = JobRepository;
			this._projectRepo = _projectRepo;
			this._freelancerRepo = _freelancerRepo;
		}

		// GET: api/<JobProposalController>/5
		[HttpGet("{jobId:int}")]
		public IActionResult Get(int jobId)
		{
			// Invalid job Id check
			var job = _jobRepo.Read(jobId);
			if (job == null)
			{
				return NotFound(new Response(404, ["Job Id is invalid"]));
			}

			// Owner of project check
			var projectOwnerId = _projectRepo.ReadProjectTakerId(job.ProjectId);
			if (projectOwnerId != GetId())
			{
				return BadRequest(new Response(401, ["Not authorized to access job proposals of a project you didn't take"]));
			}

			var proposals = _jobProposalRepo.ReadAll(jobId);
			return Ok(new Response(200, proposals));
		}

		// GET api/<JobProposalController>/5
		[HttpGet("{freelancerId}")]
		public IActionResult Get(string freelancerId)
		{
			// check Id
			freelancerId = GetId();

			var proposals = _jobProposalRepo.ReadAll(freelancerId);
			return Ok(new Response(200, proposals));
		}

		// POST api/<JobProposalController>
		[HttpPost("{jobId:int}")]
		public IActionResult Post(int jobId, [FromBody] CreateJobProposalDto jobProposalDto)
		{
			// check owner of the project
			var job = _jobRepo.Read(jobId);
			if (job == null)
			{
				return BadRequest(new Response(404, ["specified job not found"]));
			}
			var projectTakerId = _projectRepo.ReadProjectTakerId(job.ProjectId);
			if ( projectTakerId != GetId() )
			{
				return BadRequest(new Response(401, ["Not authorized to create proposals for job of a project you don't own"]));
			}

			//TODO: Payment "current is simple"
			//var senderFreelancer = _freelancerRepo.Read(projectTakerId);

			//if (senderFreelancer!.Balance < jobProposalDto.Price)
			//{
			//	return BadRequest(new Response(401, ["Cann't complete operation due insufficient Balance"]));
			//}
			//else
			//{
			//	senderFreelancer.Balance -= jobProposalDto.Price; // will not affect anything //Todo : Payment
			//}
			// ToDo: Think of approach to release this payment to freelancer when the job is compeleted
			jobProposalDto.WorkId = jobId;
			_jobProposalRepo.Create(jobProposalDto);
			return Ok(new Response(201));
		}

		// DELETE api/<JobProposalController>/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			// check owner of project 
			var proposal = _jobProposalRepo.Read(id);
			if (proposal == null)
			{
				return NotFound(new Response(404, ["Proposal Id is invalid"]));
			}
			else if (proposal.FreelancerId != GetId() || proposal.ProposalReplay?.IsAccepted == true)
			{
				return BadRequest(new Response(401, ["Not authorized to delete this proposal"]));
			}

			_jobProposalRepo.Delete(id);
			return Ok(new Response(200));
		}
	}
}
