
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using src.Data;
using src.Models;
using src.Models.Dto;
using src.Repository;
using src.Services;
using System.Text;

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
			builder.Services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "JWTToken_Auth_API",
					Version = "v1"
				});
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement {
				{
					new OpenApiSecurityScheme {
						Reference = new OpenApiReference {
							Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
						}
					},
					new string[] {}
				}
			});
			});

			builder.Services.AddDbContext<Context>(option =>
			{
				option.UseSqlServer(_config.GetConnectionString("CS"));
			});
			builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<Context>();

			builder.Services.AddScoped<AccountRepository>();
			builder.Services.AddScoped<Response>();
			builder.Services.AddScoped<CategoryRepository>();
			builder.Services.AddScoped<ProjectRepository>();
			builder.Services.AddScoped<ImageService>();
			builder.Services.AddScoped<SkillRepository>();
			builder.Services.AddScoped<ClientRepository>();
			builder.Services.AddScoped<FreelancerRepository>();
			builder.Services.AddScoped<JobRepository>();

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("PublicPolicy", options =>
				{
					options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
				});
			});

			builder.Services
				.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidIssuer = _config["JWT:Issuer"],
						ValidAudience = _config["JWT:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!)),
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateIssuerSigningKey = true,
						ValidateLifetime = true
					};
				});

			builder.Services.AddAuthorization();

			builder.Services.AddAutoMapper(typeof(MappingProfile));

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
					var admin = new ApplicationUser()
					{
						Email = _config["Admin:Email"],
						UserName = _config["Admin:Email"]
					};
					await userManager.CreateAsync(admin, _config["Admin:Password"]!);
					await userManager.AddToRoleAsync(admin, "Admin");
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

			app.UseStaticFiles();

			app.UseCors("PublicPolicy");

			app.UseAuthentication();

			app.UseAuthorization();

			app.MapControllers();

			#endregion
			
			app.Run();
		}
	}
}
