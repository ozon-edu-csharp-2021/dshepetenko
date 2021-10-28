using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MerchandiseService.Infrastructure.Middlewares
{
    public class VersionMiddleware
    {
        public VersionMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var serviceName = Assembly.GetExecutingAssembly().GetName().Name;
            var versionModel = new
            {
                Version = version.ToString(),
                ServiceName = serviceName
            };
            await context.Response.WriteAsJsonAsync(versionModel);
        }
    }
}