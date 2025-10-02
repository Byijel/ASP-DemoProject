using Ap.Demo.API.Extensions;
using Ap.Demo.Application.Extensions;
using Ap.Demo.Infrastructure.Extensions;
using DotNetEnv;

namespace Ap.Demo.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Env.Load();
			var builder = WebApplication.CreateBuilder(args);

			// Services
			builder.Services.RegisterApplication();
			            builder.Services.RegisterInfrastructure(builder.Configuration);
			            builder.Services.Configure<Ap.Demo.Application.Configuration.NotificationSettings>(builder.Configuration.GetSection("NotificationSettings"));			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowWebUi", policy =>
				{
					policy.AllowAnyHeader()
						  .AllowAnyMethod()
						  .WithOrigins("https://localhost:7294", "http://localhost:5280");
				});
			});

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseErrorHandlingMiddleware();
			app.UseCors("AllowWebUi");
			app.UseHttpsRedirection();
			app.UseAuthorization();
			app.MapControllers();
			app.Run();
		}
	}
}