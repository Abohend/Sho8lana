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
        public List<Project>? GetAllWithCategoryAndClient()
		{
			return _db.Projects
				.Include(j => j.Category)
				.Include(j => j.Client)
				.ToList();
		}
		public Project? Get(int id)
		{
			return _db.Projects.FirstOrDefault(j => j.Id == id);
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
		public bool Update(int id, CreateProjectDto newProject)
		{
			Project? project = Get(id);
			if (project != null)
			{
				project.Title = newProject.Title;
				project.Description = newProject.Description;
				project.CategoryId = newProject.CategoryId;
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
