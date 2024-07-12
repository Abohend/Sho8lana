using AutoMapper;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;
using src.Models.Dto.Freelancer;
using src.Models.Dto.Project;

namespace src.Repository
{
    public class ProjectRepository
	{
		private readonly Context _db;
		private readonly ProjectProposalRepository _projectProposalRepo;
		private readonly IMapper _mapper;

		public ProjectRepository(Context db, 
			ProjectProposalRepository projectProposalRepository,
			IMapper mapper)
        {
			_db = db;
			this._projectProposalRepo = projectProposalRepository;
			this._mapper = mapper;
		}

		#region Read
		public List<ReadProjectDto>? ReadAll()
		{
			var projects = _db.Projects
				.Include(j => j.Category)
				.Include(j => j.Client)
				.Include(p => p.Skills)
				.ToList();

			return _mapper.Map<List<ReadProjectDto>?>(projects);
		}

		public ReadProjectDto? Read(int id)
		{
			return _mapper.Map<ReadProjectDto?>(_db.Projects.FirstOrDefault(j => j.Id == id));
		}

		public ReadProjectDto? ReadWithSkills(int id)
		{
			return _mapper.Map<ReadProjectDto?>(_db.Projects
				.Include (j => j.Skills)
				.FirstOrDefault(j => j.Id == id));
		}
		
		public List<ReadProjectDto>? ReadWithSkills(string title)
		{
			// return all the projects with title like name
			return _mapper.Map<List<ReadProjectDto>?>(_db.Projects
                .Include(j => j.Skills)
                .Where(j => j.Title.Contains(title))
                .ToList());
		}

		public List<int>? ReadAll(string freelancerId)
		{
			var acceptedProposals = _projectProposalRepo.ReadAccepted(freelancerId);
			return acceptedProposals?.Select(p => p.WorkId).ToList();
		}

		public string? ReadProjectTakerId(int projectId)
		{
			var acceptedProposal = _projectProposalRepo.ReadAccepted(projectId);
			if (acceptedProposal != null)
				return acceptedProposal.FreelancerId;
			return null;
		}

		#endregion

		public void Create(string ClientId, List<Skill>? skills, CreateProjectDto projectDto)
		{
			var project = _mapper.Map<Project>(projectDto);
			project.ClientId = ClientId;
			project.Skills = skills;
			_db.Add(project);
			_db.SaveChanges();
		}

		/// <summary>
		/// Update a project if exist, or return false if not exist
		/// </summary>
		/// <param name="id"></param>
		/// <param name="newProject"></param>
		/// <returns></returns>
		public bool Update(int id, CreateProjectDto newProject)
		{
			Project? project = _db.Projects.Include(j => j.Skills).SingleOrDefault(p => p.Id == id);
			if (project != null)
			{
				project.Title = newProject.Title;
				project.Description = newProject.Description;
				project.CategoryId = newProject.CategoryId;
				if (newProject.RequiredSkillsId != null)
				{
					project.Skills = new List<Skill>();
					foreach(var skillId in newProject.RequiredSkillsId)
					{
						Skill skill = _db.Skills.Find(skillId)!;
						if (!project.Skills.Contains(skill))
						{
							project.Skills.Add(skill);
						}
					}
				}
				_db.SaveChanges();
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// Delete the project and return true if found, return false if not found
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool Delete(int id)
		{
			var project = _db.Projects.Find(id);
			if (project != null)
			{
				_db.Remove(project);
				_db.SaveChanges();
				return true;
			}
			return false;
		}

	}
}
