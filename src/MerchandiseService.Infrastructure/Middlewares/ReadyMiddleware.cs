using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MerchandiseService.Infrastructure.Middlewares
{
    public class ReadyMiddleware
    {
        public ReadyMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.StatusCode = 200;
            var status = new { status = "ready" };
            await context.Response.WriteAsJsonAsync(status);
        }
    }
}