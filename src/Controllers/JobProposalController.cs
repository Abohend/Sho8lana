using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Models.Dto.JobProposal;
using src.Repository;
using System.Security.Claims;

namespace src.Controllers
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
			ProjectRepository projectRepository,
			FreelancerRepository freelancerRepository)
		{
			this._jobProposalRepo = JobProposalRepository;
			this._jobRepo = JobRepository;
			this._projectRepo = projectRepository;
			this._freelancerRepo = freelancerRepository;
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
			var projectOwnerId = _projectRepo.Read(job.ProjectId)!.FreelancerId;
			if (projectOwnerId != GetId())
			{
				return BadRequest(new Response(401, ["Not authorized to access this job proposals"]));
			}

			var proposals = _jobProposalRepo.ReadAll(jobId);
			return Ok(new Response(200, proposals));
		}

		// GET api/<JobProposalController>/5
		[HttpGet("{freelancerId:alpha}")]
		public IActionResult Get(string freelancerId)
		{
			// check Id
			freelancerId = GetId();

			var proposals = _jobProposalRepo.ReadAll(freelancerId);
			return Ok(new Response(200, proposals));
		}

		// POST api/<JobProposalController>
		[HttpPost]
		public IActionResult Post([FromBody] CreateJobProposalDto jobProposalDto)
		{
			// check owner of the project
			var job = _jobRepo.Read(jobProposalDto.JobId);
			var project = _projectRepo.Read(job.ProjectId);
			if (project!.FreelancerId != GetId())
			{
				return BadRequest(new Response(401, "Not authorized to create proposals for job of a project you don't own"));
			}

			//TODO: Payment "current is simple"
			var senderFreelancer = _freelancerRepo.Read(project!.FreelancerId!);

			if (senderFreelancer!.Balance < jobProposalDto.Price)
			{
				return BadRequest(new Response(401, ["Cann't complete operation due insufficient Balance"]));
			}
			else
			{
				senderFreelancer.Balance -= jobProposalDto.Price;
			}	
			// ToDo: Think of approach to release this payment to freelancer when the job is compeleted
			_jobProposalRepo.Create(jobProposalDto);
			return Ok(new Response(201));
		}

		// PUT api/<JobProposalController>/5
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] RespondJobProposalDto dto)
		{
			// validation for proposalId
			var proposal = _jobProposalRepo.Read(id);
			if (proposal == null)
			{
				return NotFound(new Response(404, ["Proposal Id is invalid"]));
			}

			// only pending proposal could be modified
			if (proposal.IsAccepted != null)
			{
				return BadRequest(new Response(401, ["You already responded to this proposal"]));
			}

			// check if reciever "GetId()" == proposal.FreelancerId
			if (GetId() != proposal.FreelancerId)
			{
				return BadRequest(new Response(401, ["Not authorized to respond to this proposal"]));
			}

			_jobProposalRepo.UpdateByReciever(id, dto);
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
			else if (proposal.FreelancerId != GetId() || proposal.IsAccepted == true)
			{
				return BadRequest(new Response(401, ["Not authorized to delete this proposal"]));
			}

			_jobProposalRepo.Delete(id);
			return Ok(new Response(200));
		}
	}
}
