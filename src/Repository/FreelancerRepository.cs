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
		private readonly ProjectRepository _projectRepo;
		private readonly JobRepository _jobRepo;

		public FreelancerRepository(Context db, ImageService imageService, IMapper mapper, 
			ProjectProposalRepository projectProposalRepository, ProjectRepository projectRepository,
			JobRepository jobRepository)
		{
			this._db = db;
			this._imageService = imageService;
			this._mapper = mapper;
			this._projectProposalRepo = projectProposalRepository;
			this._projectRepo = projectRepository;
			this._jobRepo = jobRepository;
		}

		public List<ReadFreelancerDto>? Read()
		{
			var freelancers = _db.Freelancers.ToList();
			return _mapper.Map<List<ReadFreelancerDto>?>(freelancers);
		}

		public List<ReadFreelancerDto>? ReadAll()
		{
			var freelancers = _db.Freelancers.Include(f => f.Skills).ToList();
			return _mapper.Map<List<ReadFreelancerDto>?>(freelancers);
		}
		public ReadFreelancerDto? Read(string id)
		{
			var freelancer = _db.Freelancers.Find(id);
			return _mapper.Map<ReadFreelancerDto?>(freelancer);
		}
		public ReadFreelancerDto? ReadFull(string id)
		{
			var freelancer = _db.Freelancers.Include(f => f.Skills).Include(f => f.JobProposals).Include(f => f.ProjectsProposal).SingleOrDefault(f => f.Id == id);
			var freelancerDto = _mapper.Map<ReadFreelancerDto?>(freelancer);

			//freelancerDto!.Jobs = _jobRepo.ReadAll(id);
			//freelancerDto!.Projects = _projectRepo.ReadAll(id);
			

			return freelancerDto;
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
			Freelancer? freelancer = _db.Freelancers.Find(id);
			if (freelancer != null)
			{
				_imageService.DeleteImage(freelancer.ImagePath);
				_db.Remove(freelancer);
				_db.SaveChanges();
				return true;
			}
			return false;
		}
	}
}
