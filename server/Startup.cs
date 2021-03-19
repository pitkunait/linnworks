using System;
using LinnworksTechTest.Repositories.SalesRecords;
using LinnworksTechTest.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LinnworksTechTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
            
            
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var dbHost = Configuration["DBHOST"] ?? "localhost";
            var dbPort = Configuration["DBPORT"] ?? "1433";
            var dbUser = Configuration["DBUSER"] ?? "sa";
            var dbPass = Configuration["DBPASS"] ?? "Strong(!)Password1";
            var dbDatabase = Configuration["DBDATABASE"] ?? "master";
            var connectionSting = $"Server={dbHost},{dbPort};Database={dbDatabase};User={dbUser};Password={dbPass};";
            
            services.AddTransient(_ => new SalesRecordsRepository(connectionSting));
            services.AddTransient<SalesRecordsService>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
