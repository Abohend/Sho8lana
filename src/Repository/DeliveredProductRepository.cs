using AutoMapper;
using Microsoft.CodeAnalysis;
using src.Data;
using src.Models;
using src.Models.Dto;

namespace src.Repository
{
    public class DeliveredProductRepository
    {
        private readonly Context _db;
        private readonly IMapper _mapper;

        public DeliveredProductRepository(Context db, IMapper mapper)
        {
            this._db = db;
            this._mapper = mapper;
        }

        public void CreateProjectProduct(int projectId, string GithubUrl)
        {
            _db.DeliveredProjects.Add(new DeliveredProject
            {
                ProjectId = projectId,
                GitHubUrl = GithubUrl
            });
            _db.SaveChanges();
        }

        public void CreateJobProduct(int jobId, string GithubUrl)
        {
            _db.DeliveredJobs.Add(new DeliveredJob
            {
                JobId = jobId,
                GitHubUrl = GithubUrl
            });
            _db.SaveChanges();
        }

        public ReadDeliveredProductDto? ReadProjectProduct(int projectId)
        {
            return _mapper.Map<ReadDeliveredProductDto>(_db.DeliveredProjects.Find(projectId));
        }

        public ReadDeliveredProductDto? ReadJobProduct(int jobId)
        {
            return _mapper.Map<ReadDeliveredProductDto>(_db.DeliveredJobs.Find(jobId));
        }

        public void VerifyJobProduct(int jobId)
        {
            var jobProduct = ReadJobProduct(jobId);
            jobProduct!.Verified = true;
            _db.SaveChanges();
        }
        
        public void VerifyProjectProduct(int projectId)
        {
            var projectProduct = ReadProjectProduct(projectId);
            projectProduct!.Verified = true;
            _db.SaveChanges();
        }

    }
}
