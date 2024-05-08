using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Models.Dto.ProjectProposal;
using src.Repository;
using System.Security.Claims;

namespace src.Controllers
{
	[Authorize(Roles = ("Freelancer, Client"))]
	[Route("api/[controller]")]
	[ApiController]
	public class ProjectProposalController : ControllerBase
	{
		private readonly ProjectProposalRepository _projectProposalRepo;
		private readonly ProjectRepository _projectRepo;
		private readonly ClientRepository _clientRepo;

		#region Helpers
		private string GetId()
		{
			return User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
		}
		#endregion

		public ProjectProposalController(ProjectProposalRepository projectProposalRepository,
			ProjectRepository projectRepository,
			ClientRepository clientRepository)
        {
			this._projectProposalRepo = projectProposalRepository;
			this._projectRepo = projectRepository;
			this._clientRepo = clientRepository;
		}

        // GET api/<ProjectProposalController>/5
        [HttpGet("{projectId:int}")]
		public IActionResult Get(int projectId)
		{
			// Invalid project Id check
			var project = _projectRepo.Read(projectId);
			if (project == null)
			{
				return NotFound(new Response(404, ["Project Id is invalid"]));
			}
			
			// Owner of project check
			if (project.ClientId != GetId())
			{
				return BadRequest(new Response(401, ["Not authorized to respond to this project data"]));
			}

			var proposal = _projectProposalRepo.ReadAll(projectId);
			return Ok(new Response(200, proposal));
		}

		[Authorize(Roles = "Freelancer")]
        // GET api/<ProjectProposalController>/skdfja2398fashdf
        [HttpGet("{freelancerId:alpha}")]
		public IActionResult Get(string freelancerId)
		{
			// check Id
			freelancerId = GetId();

			var proposal = _projectProposalRepo.ReadAll(freelancerId);
			return Ok(new Response(200, proposal));
		}

		[Authorize(Roles = "Freelancer")]
		// POST api/<ProjectProposalController>
		[HttpPost]
		public IActionResult Post([FromBody] CreateProjectProposalDto projectProposalDto)
		{
			projectProposalDto.FreelancerId = GetId();
			_projectProposalRepo.Create(projectProposalDto);
			return Ok(new Response(201));
		}

		[Authorize(Roles  = "Client")]
		// PUT api/<ProjectProposalController>/5
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] RespondProjectProposalDto dto)
		{
			// validation for proposalId
			var proposal = _projectProposalRepo.Read(id);
			if (proposal == null)
			{
				return NotFound(new Response(404, ["Proposal Id is invalid"]));
			}

			// only pending proposal could be modified
			if (proposal.IsAccepted != null)
			{
				return BadRequest(new Response(401, ["You already responded to this proposal"]));
			}

			// check if clientId == proposal.Project.ClientId
			var clientId = _projectRepo.Read(proposal.ProjectId)!.ClientId;
			if (GetId() != clientId)
			{
				return BadRequest(new Response(401, ["Not authorized to respond to this proposal"]));
			}

			//TODO: Payment "current is simple"
			var client = _clientRepo.Read(clientId);
			if (dto.IsAccepted == true)
			{
				if (client!.Balance < proposal.OfferedPrice)
				{
					return BadRequest(new Response(401, ["Cann't complete operation due insufficient Balance"]));
				}
				client.Balance -= proposal.OfferedPrice;
				// ToDo: Think of approach to release this payment to freelancer when the project is compeleted
			}
			
			_projectProposalRepo.UpdateByClient(id, dto);
			return Ok(new Response(201));
		}

		[Authorize(Roles = "Freelancer")]
		// DELETE api/<ProjectProposalController>/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			// check owner of project 
			var proposal = _projectProposalRepo.Read(id);
			if (proposal == null)
			{
				return NotFound(new Response(404, ["Proposal Id is invalid"]));
			}
			else if (proposal.FreelancerId != GetId() || proposal.IsAccepted == true)
			{
				return BadRequest(new Response(401, ["Not authorized to delete this proposal"]));
			}

			_projectProposalRepo.Delete(id);
			return Ok(new Response(200));
		}
	}
}
