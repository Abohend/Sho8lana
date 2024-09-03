using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sho8lana.DataAccess.Data;
using Sho8lana.Entities.Models;
using Sho8lana.Entities.Models.Dto.Client;

namespace Sho8lana.DataAccess.Repositories
{
	public class ClientRepository
	{
		private readonly Context _db;
		private readonly IMapper _mapper;

		public ClientRepository(Context db, IMapper mapper)
        {
			this._db = db;
			this._mapper = mapper;
		}

		public List<ReadClientDto>? Read()
		{
			var clients = _db.Clients.ToList();
			return _mapper.Map<List<ReadClientDto>?>(clients);
		}
	
		public List<ReadClientDto>? ReadWithProjects() 
		{
			var clients = _db.Clients.Include(c => c.Projects).ToList();
			return _mapper.Map<List<ReadClientDto>?>(clients);
		}
	
		public List<ReadClientDto>? ReadWithProjectsByName(string name) 
		{
			var clients = _db.Clients
				.Include(c => c.Projects)
				.Where(c => c.Name!.Contains(name))
				.ToList();
			return _mapper.Map<List<ReadClientDto>?>(clients);
		}

		public ReadClientDto? Read(string id)
		{
			var client = _db.Clients.Find(id);
			return _mapper.Map<ReadClientDto>(client);
		}
		public ReadClientDto? ReadWithProjects(string id)
		{
			var client = _db.Clients.Include(c => c.Projects).SingleOrDefault(c => c.Id == id);
			return _mapper.Map<ReadClientDto?>(client);
		}

		public bool Update(string id, UpdateClientDto newClient, string? imagePath)
		{
			Client? client = _db.Clients.Find(id);
			if (client != null)
			{
				if(imagePath != null)
                    client.ImagePath = imagePath;
                // Note : Email will not be updated as UserName == Email "only unique values"
                client.Name = newClient.Name;
				client.PhoneNumber = newClient.PhoneNumber;
				_ = _db.SaveChanges();
				return true;
			}
			return false;
		}

		public bool UpdateBalance(string id, decimal amount)
		{
			var client = _db.Clients.SingleOrDefault(c => c.Id == id);
			if ((client!.Balance + amount) >= 0)
			{
				client.Balance += amount;
				_db.SaveChanges();
				return true;
			}
			return false;
		}

		public bool Delete(string id)
		{
			Client? client = _db.Clients.Find(id);
			if (client != null)
			{
				_db.Remove(client);
				_db.SaveChanges();
				return true;
			}
			return false;
		}
    }
}
