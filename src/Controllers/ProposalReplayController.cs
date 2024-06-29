using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Models.Dto.ProposalAndReplay;
using src.Repository;
using System.Security.Claims;

namespace src.Controllers
{
	[Authorize(Roles = "Freelancer, Client")]
	[Route("api/[controller]")]
	[ApiController]
	public class ProposalReplayController : ControllerBase
	{
		private readonly ProposalReplayRepository _proposalReplayRepo;
		private readonly ProjectProposalRepository _projectProposalRepo;
		private readonly ProjectRepository _projectRepo;
		private readonly ClientRepository _clientRepository;
		private readonly JobProposalRepository _jobProposalRepo;

		#region Helpers
		private string GetId()
		{
			return User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
		}
		#endregion

		public ProposalReplayController(ProposalReplayRepository proposalReplayRepository
			,ProjectProposalRepository projectProposalRepository
			, ProjectRepository projectRepository
			,ClientRepository clientRepository,
			JobProposalRepository jobProposalRepository)
		{
			this._proposalReplayRepo = proposalReplayRepository;
			this._projectProposalRepo = projectProposalRepository;
			this._projectRepo = projectRepository;
			this._clientRepository = clientRepository;
			this._jobProposalRepo = jobProposalRepository;
		}


		[HttpPost("/project/{projectProposalId:int}")]
		public IActionResult Post(int projectProposalId, ProposalReplayDto replayDto)
		{
			/*
			 Creating ProjectProposal Replay steps
			1. valid owner of the projectProposal GetId() == ProjectProposal.project.clientId
			2. valid that project has no replayDto before
			3. if replayDto.IsAccepted == true => make psuado payment, update freelancerId for the project
			 */
			
			// validating project proposal id
			var projectProposal = _projectProposalRepo.Read(projectProposalId);
			if (projectProposal == null)
			{
				return BadRequest(new Response(404, ["Project proposal Id not valid"]));
			}

			// validating owner of the project proposal "owner of the project"
			var project = _projectRepo.Read(projectProposal.WorkId);
			var userId = GetId();
			if (userId != project!.ClientId)
			{
				return BadRequest(new Response(401, ["Only can replay for proposals of your projects"]));
			}

			// valid the project is not replied before
			if (projectProposal.ProposalReplay != null)
			{
				return BadRequest(new Response(401, ["This project has been responded before"]));
			}

			// Psuado Payment
			//if (replayDto.IsAccepted == true)
			//{
			//	var result = _clientRepository.UpdateBalance(userId, -projectProposal.Price); // Todo: Will be changed later "Payment Gateway process"
			//	if (!result)
			//	{
			//		return BadRequest(new Response(400, ["Cann't complete due to insufficient balance"])); 
			//	}
			
			//}

			// Create Replay
			_proposalReplayRepo.Create(replayDto, projectProposalId);
			return Ok(new Response(200));
		}


		[HttpPost("/job/{jobProposalId:int}")]
		public IActionResult PostJobProposalReplay(int jobProposalId, ProposalReplayDto replayDto)
		{
			/*
			 Creating JobProposal Replay steps
			1. valid owner of the JobProposal GetId() == JobProposal.FreelancerId
			2. valid that job has no replayDto before
			3. if replayDto.IsAccepted == ture => make psaudo payment
			 */

			// validating job proposal Id
			var job = _jobProposalRepo.Read(jobProposalId);

			if (job == null)
			{
				return BadRequest(new Response(401, ["Invalid job proposal Id"]));
			}

			// validating that he's the proposal reciever
			if (GetId() != job.FreelancerId)
			{
				return BadRequest(new Response(401, ["You cann't reply to a proposal you didn't own"]));
			}

			_proposalReplayRepo.Create(replayDto, jobProposalId);
			return Ok(new Response(200));
		}
	}
}
