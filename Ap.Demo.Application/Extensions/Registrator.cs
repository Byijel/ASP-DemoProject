using Ap.Demo.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AutoMapper;

namespace Ap.Demo.Application.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection RegisterApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}