using Clients.AuthApi;
using Clients.ProductApi;
using System.Text;

namespace CustomGateway.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _environment = environment;
            _logger = logger;
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
            catch (ProductApiException apiEx)
            {
                if (apiEx.StatusCode == 200)
                {
                    throw new ApplicationException($"Got {nameof(ProductApiException)} with status code 200:", apiEx);
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
