
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;
using src.Repository;

namespace src
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var _config = builder.Configuration;

			#region Services
			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<Context>(option =>
			{
				option.UseSqlServer(_config.GetConnectionString("CS"));
			});
			builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<Context>();

			builder.Services.AddScoped<AccountRepository>();
			builder.Services.AddScoped<Response>();
			#endregion

			var app = builder.Build();

			#region Seed roles & default admin
			using (var scope = app.Services.CreateScope())
			{
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				if (!await roleManager.RoleExistsAsync("Admin"))
				{
					string[] roles = { "Admin", "Client", "Freelancer" };
					foreach (var role in roles)
					{
						await roleManager.CreateAsync(new IdentityRole(role));
					}
				}

				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
				if (await userManager.FindByEmailAsync(_config["Admin:Email"]!) == null)
				{
					await userManager.CreateAsync(new ApplicationUser()
					{
						Email = _config["Admin:Email"],
						UserName = _config["Admin:Email"]
					}, _config["Admin:Password"]!);
				}
			}
			#endregion

			#region Middlewares
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			#endregion
			
			app.Run();
		}
	}
}
