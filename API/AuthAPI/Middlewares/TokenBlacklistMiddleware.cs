using Application.Features.Tokens.Requests.Queries;
using MediatR;
using Microsoft.Extensions.Primitives;

namespace AuthAPI.Middlewares
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMediator _mediator;

        public TokenBlacklistMiddleware(RequestDelegate next, IMediator mediator)
        {
            _mediator = mediator;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.Authorization.Any())
            {
                var token = context.Request.Headers.Authorization.ToString().Split(' ')[1];

                var result = await _mediator.Send(new IsTokenValidRequest { IsTokenValidDto = new() { Token = token } });
                
                if (result.Success && result.Result == false)
                {
                    context.Request.Headers.Authorization = StringValues.Empty;
                }
            }
            await _next(context);
        }
    }
}
