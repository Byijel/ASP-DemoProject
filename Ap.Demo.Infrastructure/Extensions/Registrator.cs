using Ap.Demo.Infrastructure.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Ap.Demo.Infrastructure.Repositories;
using Ap.Demo.Application.Interfaces;
using Ap.Demo.Infrastructure.UoW;

namespace Ap.Demo.Infrastructure.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
        {
            services.RegisterDbContext();

            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();

            services.AddScoped<IUnitofWork, UnitofWork>();
            //services.AddScoped<IEmailService, EmailService>();

            return services;
        }

        public static IServiceCollection RegisterDbContext(this IServiceCollection services)
        {
            services.AddDbContext<MyCitiesContext>(options =>
                       options.UseSqlServer("name=ConnectionStrings:MyCities"));
            return services;
        }
    }
}