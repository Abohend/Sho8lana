using AutoMapper;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;
using src.Models.Dto.Client;
using src.Services;

namespace src.Repository
{
	public class ClientRepository
	{
		private readonly Context _db;
		private readonly IMapper _mapper;
		private readonly ImageService _imageService;

		public ClientRepository(Context db, IMapper mapper, ImageService imageService)
        {
			this._db = db;
			this._mapper = mapper;
			this._imageService = imageService;
		}

		public List<GetClientDto>? Read()
		{
			var clients = _db.Clients.ToList();
			return _mapper.Map<List<GetClientDto>?>(clients);
		}
		public List<GetClientDto>? ReadWithProjects() 
		{
			var clients = _db.Clients.Include(c => c.Projects).ToList();
			return _mapper.Map<List<GetClientDto>?>(clients);
		}
		public GetClientDto? Read(string id)
		{
			var client = _db.Clients.Find(id);
			return _mapper.Map<GetClientDto>(client);
		}
		public GetClientDto? ReadWithProjects(string id)
		{
			var client = _db.Clients.Include(c => c.Projects).SingleOrDefault(c => c.Id == id);
			return _mapper.Map<GetClientDto?>(client);
		}

		public bool Update(string id, UpdateClientDto newClient)
		{
			var client = Read(id);
			if (client != null)
			{
				//image
				if (newClient.Image != null)
				{
					_imageService.DeleteImage(client.ImageUrl);
					client.ImageUrl = _imageService.UploadImage("client", newClient.Image);
				}
				
				// Note : Email will not be updated as UserName == Email "only unique values"
				client.Name = newClient.Name;
				client.PhoneNumber = newClient.PhoneNumber;
				_db.SaveChanges();
				return true;
			}
			return false;
		}

		public bool Delete(string id)
		{
			var client = Read(id);
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
