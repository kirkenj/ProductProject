using Infrastructure.TockenTractker;
using Microsoft.Extensions.Primitives;

namespace AuthAPI.Middlewares
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenTracker _tracker;

        public TokenBlacklistMiddleware(RequestDelegate next, TokenTracker mediator)
        {
            _tracker = mediator;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.Authorization.Any())
            {
                var token = context.Request.Headers.Authorization.ToString().Split(' ')[1];
                                
                if (_tracker.IsValid(token) == false)
                {
                    context.Request.Headers.Authorization = StringValues.Empty;
                }
            }
            await _next(context);
        }
    }
}
