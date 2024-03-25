using AutoMapper;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;
using src.Models.Dto;

namespace src.Repository
{
	public class JobRepository
	{
		private readonly Context _db;

		public JobRepository(Context db)
        {
			_db = db;
		}
        public List<Job>? GetAllWithCategoryAndClient()
		{
			return _db.Jobs
				.Include(j => j.Category)
				.Include(j => j.Client)
				.ToList();
		}
		public Job? Get(int id)
		{
			return _db.Jobs.FirstOrDefault(j => j.Id == id);
		}
		public Job? GetWithCategoryAndClient(int id)
		{
			return _db.Jobs
				.Include (j => j.Category)
				.Include (j => j.Client)
				.FirstOrDefault(j => j.Id == id);
		}

		public void Create(Job job)
		{
			_db.Add(job);
			_db.SaveChanges();
		}
		/// <summary>
		/// Update a job if exist, or return false if not exist
		/// </summary>
		/// <param name="id"></param>
		/// <param name="newJob"></param>
		/// <returns></returns>
		public bool Update(int id, CreateJobDto newJob)
		{
			Job? job = Get(id);
			if (job != null)
			{
				job.Title = newJob.Title;
				job.Description = newJob.Description;
				job.CategoryId = newJob.CategoryId;
				_db.SaveChanges();
				return true;
			}
			return false;
		}
		/// <summary>
		/// Delete the job and return true if found, return false if not found
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool Delete(int id)
		{
			var job = Get(id);
			if (job != null)
			{
				_db.Remove(job);
				_db.SaveChanges();
				return true;
			}
			return false;
		}

	}
}
