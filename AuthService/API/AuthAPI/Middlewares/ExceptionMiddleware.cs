
namespace AuthAPI.Middlewares
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
                await _next(context);
            try
            {
            }
            catch(Exception ex) 
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(_environment.IsDevelopment() ? ex.Message : "Ooopsie", typeof(string));
            }
        }
    }
}
