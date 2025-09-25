using Ap.Demo.Infrastructure.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Ap.Demo.Application.Interfaces;
using Ap.Demo.Infrastructure.UoW;
using Ap.Demo.Infrastructure.Repositories;

namespace Ap.Demo.Infrastructure.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
        {
            services.RegisterDbContext();
            services.RegisterRepositories();
            return services;
        }
        public static IServiceCollection RegisterDbContext(this IServiceCollection services)
        {
            services.AddDbContext<MyCitiesContext>(options =>
            {
                options.UseSqlServer("name=ConnectionStrings:FirstConnection");
            });

            return services;

        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICityRepository, CityRepository>();
            /*services.AddScoped<IStoreRepository, StoreRepository>();*/
            services.AddScoped<IUnitofWork, UnitofWork>();
            return services;
        }
    }
}