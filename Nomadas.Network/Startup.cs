using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.EntityFrameworkCore;
using Nomadas.Network.Core;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Server.Kestrel.Core;

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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);
            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddScoped<AppSchema>();
            services.AddGraphQL(o => { o.ExposeExceptions = false; }).AddGraphTypes(ServiceLifetime.Scoped);

            services.AddControllers();
            services.AddHealthChecks();
        }

        private static string getDBConnectionString()
        {
            var server = Environment.GetEnvironmentVariable("DB_SERVER");
            var pass = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var user = Environment.GetEnvironmentVariable("ADMIN_LOGIN");
            var dbname = Environment.GetEnvironmentVariable("DB_NAME");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var conn = $"Server=tcp:{server},{port};Initial Catalog={dbname};User ID={user};Password={pass};";
            // conn += server != "localhost" ? "Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" : "";
            // Console.WriteLine(conn);
            return conn;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            setErrorMessageSystem(app, env);

            // app.UseHttpsRedirection();
            // app.UseAuthorization();

            app.UseGraphQL<AppSchema>();
            app.UseGraphQLPlayground(options: new GraphQLPlaygroundOptions());

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", context => context.Response.WriteAsync("Hello world"));
                endpoints.MapGet("/connection", context => context.Response.WriteAsync(getDBConnectionString()));
                endpoints.MapGet("/crash", context => throw new Exception("Boom!"));
                endpoints.MapHealthChecks("/health");
            });
        }

        private static void setErrorMessageSystem(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsStaging())
            {
                app.UseExceptionHandler("/error-staging");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
        }
    }
}
