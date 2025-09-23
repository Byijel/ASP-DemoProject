using Ap.Demo.WebUi.Components;
using Microsoft.EntityFrameworkCore;
using Ap.Demo.Infrastructure.Contexts;
using Ap.Demo.Infrastructure.Extensions;
using Ap.Demo.Application.Extensions;
using AutoMapper;


namespace Ap.Demo.WebUi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Configureer de DbContext
            builder.Services.AddDbContext<MyCitiesContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyCities")));

            builder.Services.RegisterInfrastructure();
            builder.Services.RegisterApplication();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
