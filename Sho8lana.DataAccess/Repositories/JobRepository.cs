﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sho8lana.DataAccess.Data;
using Sho8lana.Entities.Models;
using Sho8lana.Entities.Models.Dto.Job;

namespace Sho8lana.DataAccess.Repositories
{
	public class JobRepository
	{
		private readonly Context _context;
		private readonly IMapper _mapper;
        private readonly ProjectRepository projectRepo;

        public JobRepository(Context context, IMapper mapper, ProjectRepository projectRepo)
        {
			this._context = context;
			this._mapper = mapper;
            this.projectRepo = projectRepo;
        }

		public void Create(int projectId,List<CreateJobDto> JobsDto)
		{
			var jobs = _mapper.Map<List<Job>>(JobsDto);
			foreach (var job in jobs)
			{
				job.ProjectId = projectId;
				_context.Add(job);
			}
			_context.SaveChanges();
		}

		public ReadJobDto Read(int id)
		{
			var job = _context.Jobs.Find(id);
			return _mapper.Map<ReadJobDto>(job);
		}

		public List<ReadJobDto>? ReadAll(int projectId)
		{
			var jobs = _context.Jobs.Where(j => j.ProjectId == projectId).ToList();
			return _mapper.Map<List<ReadJobDto>?>(jobs);
		}

		public List<ReadJobDto>? ReadAll(string freelancerId)
		{
			var jobs = _context
				.Jobs
				.Include(p => p.Proposals)
				//.Where(p => ReadJobTaker(p) == freelancerId)
				.Where(j => j.Proposals!.First(p => p.ProposalReplay!.IsAccepted == true).FreelancerId == freelancerId)
				.ToList();
			return _mapper.Map<List<ReadJobDto>?>(jobs);
		}

		public string ReadJobOwnerId(int jobId)
        {
            var job = _context.Jobs.Single(j => j.Id == jobId);
			// owner of the job is the project taker
            return projectRepo.ReadProjectTakerId(job.ProjectId)!;
        }

		public string? ReadJobTakerId(int jobId)
		{
			var job = _context.Jobs
				.Include(j => j.Proposals)!
				.ThenInclude(p => p.ProposalReplay)
				.First(j => j.Id == jobId);
			if (job.Proposals != null && job.Proposals.First(p => p.ProposalReplay?.IsAccepted == true) != null)
			{
				return job.Proposals.First(p => p.ProposalReplay?.IsAccepted == true).FreelancerId;
			}
			return null;
		}

		public bool Delete(int id)
		{
			var job = _context.Jobs.Find(id);
			if (job != null)
			{
				_context.Remove(job);
				_context.SaveChanges();
				return true;
			}
			return false;
		}
    }
}
