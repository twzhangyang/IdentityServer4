using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityServer4.AmazonDynamoDB.Storage.Configuration;
using IdentityServer4.AmazonDynamoDB.Storage.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Samples.ResourceOwnerClient.Server
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DynamoDBOptions>(Configuration.GetSection("DynamoDB"));
            services.AddConfigurationDbContext(Configuration,
                options =>
                {
                    var section = Configuration.GetSection("DynamoDB");
                    section.Bind(options);
                });
            
            // uncomment, if you wan to add an MVC-based UI
            //services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            const string connectionString =
                @"Server=tcp:127.0.0.1,1433;Database=IdentityServer;User Id=sa;Password=RestAirline123";

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var builder = services.AddIdentityServer()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = x =>
                        x.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
//                .AddOperationalStore(options =>
//                {
//                    options.ConfigureDbContext = x =>
//                        x.UseSqlServer(connectionString,
//                            sql => sql.MigrationsAssembly(migrationsAssembly));
//
//                    // this enables automatic token cleanup. this is optional.
////                        options.EnableTokenCleanup = true;
////                        options.TokenCleanupInterval = 30; // interval in seconds
//                })
                
//                .AddInMemoryIdentityResources(Config.GetIdentityResources())
//                .AddInMemoryApiResources(Config.GetApis())
//                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers());

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }
        }

        public void Configure(IApplicationBuilder app)
        {
//            DatabaseMigration.InitializeDatabase(app);

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to support static files
            //app.UseStaticFiles();

            app.UseIdentityServer();

            // uncomment, if you wan to add an MVC-based UI
            //app.UseMvcWithDefaultRoute();
        }
    }
}