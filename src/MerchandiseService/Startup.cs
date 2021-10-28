using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MerchandiseService.Infrastructure.Interceptors;
using MerchandiseService.Infrastructure.Middlewares;
using MerchandiseService.Services.Interfaces;
using MerchandiseService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;


namespace MerchandiseService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMerchandiseService, MerchService>();

            services.AddGrpc(options => options.Interceptors.Add<LoggingInterceptor>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}