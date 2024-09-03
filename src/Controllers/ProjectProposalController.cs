﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sho8lana.Entities.Models;
using Sho8lana.DataAccess.Repositories;
using System.Security.Claims;
using Sho8lana.Entities.Models.Dto.Proposal;

namespace Sho8lana.API.Controllers
{
	[Authorize(Roles = ("Freelancer, Client"))]
	[Route("api/[controller]")]
	[ApiController]
	public class ProjectProposalController : ControllerBase
	{
		private readonly ProjectProposalRepository _projectProposalRepo;
		private readonly ProjectRepository _projectRepo;
		private readonly ClientRepository _clientRepo;
		private readonly FreelancerRepository _freelancerRepo;

		#region Helpers
		private string GetId()
		{
			return User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
		}
		#endregion

		public ProjectProposalController(ProjectProposalRepository projectProposalRepository,
			ProjectRepository projectRepository,
			ClientRepository clientRepository,
			FreelancerRepository freelancerRepository)
		{
			this._projectProposalRepo = projectProposalRepository;
			this._projectRepo = projectRepository;
			this._clientRepo = clientRepository;
			this._freelancerRepo = freelancerRepository;
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
		[HttpGet("{freelancerId}")]
		public IActionResult Get(string freelancerId) 
		{ 
			// check Id
			freelancerId = GetId();

			var proposal = _projectProposalRepo.ReadAll(freelancerId);
			return Ok(new Response(200, proposal));
		}

		[Authorize(Roles = "Freelancer")]
		// POST api/<ProjectProposalController>
		[HttpPost("{projectId}")]
		public IActionResult Post(int projectId, [FromBody] CreateProposalDto projectProposalDto)
		{
			var freelancerId = GetId();

			// Verify freelancer category == project category
			var freelancer = _freelancerRepo.Read(freelancerId);
			var project = _projectRepo.Read(projectId);
			
			if (project == null)
			{
				return BadRequest(new Response(404, ["Invalid Project Id"]));
			}

			else if (freelancer!.CategoryId != project.CategoryId)
			{
				return BadRequest(new Response(404, ["Cann't take a project not in your category"]));
			}

			projectProposalDto.FreelancerId = freelancerId;
			projectProposalDto.WorkId = projectId;
			_projectProposalRepo.Create(projectProposalDto);
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
			else if (proposal.FreelancerId != GetId() || proposal.ProposalReplay != null)
			{
				return BadRequest(new Response(401, ["Not authorized to delete this proposal"]));
			}

			_projectProposalRepo.Delete(id);
			return Ok(new Response(200));
		}
	}
}
