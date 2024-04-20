using AutoMapper;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;
using src.Models.Dto.Project;

namespace src.Repository
{
    public class ProjectRepository
	{
		private readonly Context _db;

		public ProjectRepository(Context db)
        {
			_db = db;
		}
        public List<Project>? GetFullData()
		{
			return _db.Projects
				.Include(j => j.Category)
				.Include(j => j.Client)
				.Include(p => p.Skills)
				.ToList();
		}
		public Project? Get(int id)
		{
			return _db.Projects.FirstOrDefault(j => j.Id == id);
		}
		public Project? GetWithSkills(int id)
		{
			return _db.Projects.Include(j => j.Skills).SingleOrDefault(p => p.Id == id);
		}
		public Project? GetWithCategoryAndClient(int id)
		{
			return _db.Projects
				.Include (j => j.Category)
				.Include (j => j.Client)
				.FirstOrDefault(j => j.Id == id);
		}

		public void Create(Project prject)
		{
			_db.Add(prject);
			_db.SaveChanges();
		}
		/// <summary>
		/// Update a project if exist, or return false if not exist
		/// </summary>
		/// <param name="id"></param>
		/// <param name="newProject"></param>
		/// <returns></returns>
		public bool Update(int id, Project newProject)
		{
			Project? project = GetWithSkills(id);
			if (project != null)
			{
				project.Title = newProject.Title;
				project.Description = newProject.Description;
				project.CategoryId = newProject.CategoryId;
				if (newProject.Skills != null)
				{
					project.Skills = new List<Skill>();
					foreach(var skill in newProject.Skills)
					{
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
			var project = Get(id);
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
