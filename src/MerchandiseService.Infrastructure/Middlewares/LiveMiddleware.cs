using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MerchandiseService.Infrastructure.Middlewares
{
    public class LiveMiddleware
    {
        public LiveMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.StatusCode = 200;
            var status = new { status = "live" };
            await context.Response.WriteAsJsonAsync(status);
        }
    }
}