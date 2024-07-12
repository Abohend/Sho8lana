using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Models.Dto.Client;
using src.Repository;
using src.Services;
using System.Security.Claims;

namespace src.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClientController : ControllerBase
	{
		private readonly ClientRepository _clientRepo;

		public ClientController(ClientRepository clientRepository)
		{
			this._clientRepo = clientRepository;
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

		// GET: api/<ClientController>
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public IActionResult Get()
		{
			var clients = _clientRepo.ReadWithProjects();

			// update image pathes
			foreach (var client in clients!)
			{
				client.ImageUrl = GetImageUrl(client.ImageUrl);
			}

			return Ok(new Response(200, clients));
		}


		[HttpGet("search/{name}")]
		public IActionResult GetByName(string name)
		{
			var clients = _clientRepo.ReadWithProjectsByName(name);

			// update image pathes
			foreach (var client in clients!)
			{
				client.ImageUrl = GetImageUrl(client.ImageUrl);
			}

			return Ok(new Response(200, clients));
		}

		// GET api/<ClientController>/5
		[HttpGet("{id}")]
		public IActionResult Get(string id)
		{
			var client = _clientRepo.ReadWithProjects(id);
			if (client != null)
			{
				client.ImageUrl = GetImageUrl(client.ImageUrl);
				return Ok(new Response(200, client));
			}
			return BadRequest(new Response(404, ["Client not found"]));
		}

		[Authorize(Roles = "Client")]
		// PUT api/<ClientController>/5
		[HttpPut("{id}")]
		public IActionResult Put(string id, [FromForm] UpdateClientDto clientDto)
		{
			if (GetId() != id)
			{
				return Unauthorized(new Response(StatusCodes.Status203NonAuthoritative, ["Not authorized"]));
			}
			_ = _clientRepo.Update(id, clientDto);
			return Ok(new Response(200));
		}

		[Authorize(Roles = "Admin, Client")]
		// DELETE api/<ClientController>/5
		[HttpDelete("{id}")]
		public IActionResult Delete(string id)
		{
			if (GetId() == id || GetRole() == "Admin")
			{
				_ = _clientRepo.Delete(id);
				return Ok(new Response(200));
			}
			else
			{
				return Unauthorized(new Response(StatusCodes.Status203NonAuthoritative, ["Not authorized"]));
			}
		}
	}
}
