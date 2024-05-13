using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using src.Models;

namespace src.Data
{
	public class Context: IdentityDbContext<ApplicationUser>
	{
        public Context(DbContextOptions options) : base(options) { }
		public DbSet<Client> Clients { get; set; }
		public DbSet<Freelancer> Freelancers {  get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<Skill> Skills { get; set; }
		public DbSet<Job> Jobs { get; set; }
		public DbSet<ProjectProposal> ProjectsProposal { get; set; }
		public DbSet<JobProposal> JobsProposal { get; set; }
		public DbSet<ProposalReplay> ProposalReplay { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Category>()
				.HasIndex(c => c.Name)
				.IsUnique();

			builder.Entity<Skill>()
				.HasIndex(s => s.Name)
				.IsUnique();

			builder.Entity<Project>()
				.OwnsOne(j => j.ExpectedDuration);

			builder.Entity<ProposalReplay>()
				.HasKey(p => p.ProposalId); // weak entity

			builder.Entity<JobProposal>()
				.HasOne(j => j.ProposalReplay)
				.WithOne(r => r.Proposal as JobProposal)
				.HasForeignKey<ProposalReplay>(r => r.ProposalId);

			builder.Entity<ProjectProposal>()
				.HasOne(j => j.ProposalReplay)
				.WithOne(r => r.Proposal as ProjectProposal)
				.HasForeignKey<ProposalReplay>(r => r.ProposalId);

			base.OnModelCreating(builder);
		}
	}
}
