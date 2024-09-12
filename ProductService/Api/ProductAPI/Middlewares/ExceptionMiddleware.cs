using FluentValidation;

namespace ProductAPI.Middlewares
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
            catch (ValidationException valEx)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(valEx.Message);
                return;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500; 
                await context.Response.WriteAsJsonAsync(_environment.IsDevelopment() ? ex.Message : "Ooopsie", typeof(string));

            }
        }
    }
}
