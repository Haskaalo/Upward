using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Upward.Models.Database;
using Microsoft.EntityFrameworkCore;
using Google.Cloud.Storage.V1;

namespace Upward
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<upwardContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("PGUpwardDatabase"))
            );
            services.AddDbContext<accountsContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("PGAccountDatabase"))
            );
            services.AddSingleton(StorageClient.Create());
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            // var db = serviceProvider.GetService<upwardContext>();
            // db.Database.Migrate();
        }
    }
}
