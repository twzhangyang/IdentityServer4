using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Samples.ClientCredential.Server
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public IHostingEnvironment Environment { get; }

        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            _configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you wan to add an MVC-based UI
            //services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients());

            if (Environment.IsDevelopment())
            {
                LoadCert(builder);

             //   builder.AddDeveloperSigningCredential();
            }
            else
            {
                LoadCert(builder);
            }
            
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(TimeSpan.FromSeconds(3));

        }

        private void LoadCert(IIdentityServerBuilder builder)
        {
            var fileName = Path.Combine(Directory.GetCurrentDirectory(), "jedis-test-auth.pfx");

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Signing Certificate is missing!");
            }
//            var cert = new X509Certificate2(fileName, "123", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
//            var bytes = cert.Export(X509ContentType.Pkcs12);
//            var base64 = Convert.ToBase64String(bytes);


            var value = _configuration["Cert"];
            var bytes = Convert.FromBase64String(value);
            var cert = new X509Certificate2(bytes,"123", X509KeyStorageFlags.MachineKeySet);

            builder.AddSigningCredential(cert);
        }

        public void Configure(IApplicationBuilder app)
        {
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