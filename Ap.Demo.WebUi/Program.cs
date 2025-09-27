using Ap.Demo.Application.Extensions;
using Ap.Demo.Infrastructure.Contexts;
using Ap.Demo.Infrastructure.Extensions;
using Ap.Demo.WebUi.Components;
using Ap.Demo.WebUi.Services;
using Microsoft.EntityFrameworkCore;

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

            builder.Services.AddScoped<CityService>();
            builder.Services.AddHttpClient<CityService>(client => client.BaseAddress = new Uri("https://localhost:7217/"));

            //Add controllers and HttpClient
            builder.Services.AddControllers();
            builder.Services.AddHttpClient();

            // Register Application and Infrastructure services
            builder.Services.RegisterApplication();
            builder.Services.RegisterInfrastructure(builder.Configuration);

            // HttpClient to call API
            builder.Services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
            });

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

            app.MapControllers();

            app.Run();
        }
    }
}
