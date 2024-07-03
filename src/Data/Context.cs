using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using src.Models;
using System.Reflection.Emit;

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
		#region chat
		public DbSet<Message> Messages { get; set; }
		public DbSet<GroupChat> GroupChats { get; set; } 
		#endregion
		
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

			#region chat schema
			builder.Entity<Message>()
					.ToTable("Messages", "chat");

			builder.Entity<GroupChat>()
				.ToTable("GroupChats", "chat");

			builder.Entity<OnlineUser>()
				.ToTable("OnlineUsers", "chat");

            // Configure ApplicationUser and GroupChat many-to-many relationship
            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Chats)
                .WithMany(e => e.Members)
                .UsingEntity(j => j.ToTable("UserGroupChats"));

            // Configure GroupChat Admin relationship
            builder.Entity<GroupChat>()
                .HasOne(e => e.Admin)
                .WithMany()
                .HasForeignKey(e => e.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Message Sender and Receiver relationships
            builder.Entity<Message>()
                .HasOne(e => e.Sender)
                .WithMany(e => e.MessagesSent)
                .HasForeignKey(e => e.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(e => e.ReceiverUser)
                .WithMany(e => e.MessagesReceived)
                .HasForeignKey(e => e.ReceiverUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(e => e.ReceiverGroup)
                .WithMany(e => e.Messages)
                .HasForeignKey(e => e.ReceiverGroupId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            base.OnModelCreating(builder);
		}
	}
}
