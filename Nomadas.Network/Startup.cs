using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.EntityFrameworkCore;

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
            string connection = getDBConnectionString();
            Console.WriteLine(connection);
            // services.AddDbContext<DBContext>(options => options.UseSqlServer(connection));
            services.AddControllers();
            services.AddHealthChecks();
        }

        private static string getDBConnectionString()
        {
            var server = Environment.GetEnvironmentVariable("DB_SERVER");
            var pass = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var user = Environment.GetEnvironmentVariable("ADMIN_LOGIN");
            var dbname = Environment.GetEnvironmentVariable("DB_NAME");
            var connection = $"Server={server};Database={dbname};User={user};Password={pass};";
            return connection;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", context => context.Response.WriteAsync("Hello world " + getDBConnectionString()));
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
