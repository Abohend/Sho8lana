using src.Models;

namespace src.Repository
{
	public class TeamRepository
	{
		private readonly JobRepository _jobRepo;

		public TeamRepository(JobRepository jobRepository)
        {
			this._jobRepo = jobRepository;
		}

        public void Read(int projectId)
		{
			var team = new Team { ProjectId = projectId };

			// all jobs for the same project
			var Jobs = _jobRepo.ReadAll(projectId);

			// Freelancers that will do the jobs
			if (Jobs != null)
				team.MembersId = Jobs.Select(j => _jobRepo.ReadJobTaker(j.Id)).ToList();
		}
	}
}
