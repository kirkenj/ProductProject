using Clients.AuthClientService;
using System.Text;

namespace CustomGateway.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;

        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        {
            _next = next;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AuthApiException apiEx)
            {
                if (apiEx.StatusCode == 200)
                {
                    throw new ApplicationException($"Got {nameof(AuthApiException)} with status code 200:", apiEx);
                }

                context.Response.StatusCode = apiEx.StatusCode;
                await context.Response.WriteAsJsonAsync(apiEx.Response);
                return;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes(ex.Message));
            }
        }
    }
}
