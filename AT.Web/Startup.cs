using AT.Data.Models;
using AT.Middleware;
using AT.Repo.ConnectionFactory;
using AT.Repo.Interfaces;
using AT.Repo.Repositories;
using AT.Service.Interfaces;
using AT.Service.Services;
using AT.Web.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;

namespace AT.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<VehicleContext>(opts => opts.UseSqlServer(Configuration["ConnectionStrings:VehicleDB"]));
            services.AddScoped<IVehicleRepository<Vehicle>, VehicleRepository>();
            services.AddScoped<IVehicleService<Vehicle>, VehicleService>();
            services.AddScoped<ILoginRepository<User>, LoginRepository>();
            services.AddScoped<IConnectionFactory, ConnectionFactory>();

            services.AddControllers();

            services.AddCors(options => {
                options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyHeader());
            });

            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
            }

            app.UseCors("AllowAll");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<CustomExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Vehicle",
                    pattern: "{controller}/{action}/{id?}",
                    defaults: new { action = "Get" });
            });
        }
    }
}
