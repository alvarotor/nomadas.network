using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.EntityFrameworkCore;
using Nomadas.Network.Core;

namespace Nomadas.Network
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
            services.AddScoped<IWeatherForecastCore, WeatherForecastCore>();

            string connection = getDBConnectionString();
            services.AddDbContext<DBContext>(options => options.UseSqlServer(connection));
            services.AddControllers();
            services.AddHealthChecks();
        }

        private static string getDBConnectionString()
        {
            var server = Environment.GetEnvironmentVariable("DB_SERVER");
            var pass = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var user = Environment.GetEnvironmentVariable("ADMIN_LOGIN");
            var dbname = Environment.GetEnvironmentVariable("DB_NAME");
            return $"Server=tcp:{server},1433;Initial Catalog={dbname};Persist Security Info=False;User ID={user};Password={pass};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();
            // app.UseAuthorization();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", context => context.Response.WriteAsync("Hello world"));
                endpoints.MapGet("/connection", context => context.Response.WriteAsync(getDBConnectionString()));
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
