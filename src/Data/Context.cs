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
    }
}
