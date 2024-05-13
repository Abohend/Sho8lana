using src.Data;
using src.Models;
using Microsoft.EntityFrameworkCore;
using src.Services;
using src.Models.Dto.Freelancer;
using AutoMapper;

namespace src.Repository
{
	public class FreelancerRepository
	{
		private readonly Context _db;
		private readonly ImageService _imageService;
		private readonly IMapper _mapper;
		private readonly ProjectProposalRepository _projectProposalRepo;

		public FreelancerRepository(Context db, ImageService imageService, IMapper mapper, 
			ProjectProposalRepository projectProposalRepository)
		{
			this._db = db;
			this._imageService = imageService;
			this._mapper = mapper;
			this._projectProposalRepo = projectProposalRepository;
		}

		public List<GetFreelancerDto>? Read()
		{
			var freelancers = _db.Freelancers.ToList();
			return _mapper.Map<List<GetFreelancerDto>?>(freelancers);
		}
		public List<GetFreelancerDto>? ReadAllWithSkills()
		{
			var freelancers = _db.Freelancers.Include(f => f.Skills).ToList();
			return _mapper.Map<List<GetFreelancerDto>?>(freelancers);
		}
		public GetFreelancerDto? Read(string id)
		{
			var freelancer = _db.Freelancers.Find(id);
			return _mapper.Map<GetFreelancerDto?>(freelancer);
		}
		public GetFreelancerDto? ReadWithSkills(string id)
		{
			var freelancer = _db.Freelancers.Include(f => f.Skills).SingleOrDefault(f => f.Id == id);
			return _mapper.Map<GetFreelancerDto?>(freelancer);
		}

		public void Update(string id, UpdateFreelancerDto freelancerDto, List<Skill>? skills)
		{
			Freelancer freelancer = _db.Freelancers.Include(f => f.Skills).SingleOrDefault(f => f.Id == id)!;
			// update image
			if (freelancerDto.Image != null)
			{
				_imageService.DeleteImage(freelancer.ImagePath);
				freelancer.ImagePath = _imageService.UploadImage("freelancer", freelancerDto.Image);
			}

			//update skills
			if (skills != null)
			{
				freelancer.Skills = new(); // reset and reinitalizes with the new list
				foreach (var skill in skills)
				{
					if (!freelancer.Skills.Contains(skill))
					{
						freelancer.Skills.Add(skill);
					}
				}
			}

			// Update other attributes
			freelancer.Name = freelancerDto.Name;
			freelancer.PhoneNumber = freelancerDto.PhoneNumber;
			if (freelancerDto.CategoryId != null)
				freelancer.CategoryId = freelancerDto.CategoryId.Value;
			_db.SaveChanges();

		}

		public bool Delete(string id)
		{
			var freelancer = Read(id);
			if (freelancer != null)
			{
				_imageService.DeleteImage(freelancer.ImageUrl);
				_db.Remove(freelancer);
				_db.SaveChanges();
				return true;
			}
			return false;
		}
	}
}
