using Ap.Demo.Infrastructure.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Ap.Demo.Application.Interfaces;
using Ap.Demo.Infrastructure.UoW;
using Ap.Demo.Infrastructure.Services;
using Ap.Demo.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Ap.Demo.Infrastructure.Repositories;

namespace Ap.Demo.Infrastructure.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDbContext(configuration);
            // Configure SMTP settings
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

            services.RegisterRepositories();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
        public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MyCitiesContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("FirstConnection"));
            });

            return services;
        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            /*services.AddScoped<IStoreRepository, StoreRepository>();*/
            services.AddScoped<IUnitofWork, UnitofWork>();
            return services;
        }
    }
}