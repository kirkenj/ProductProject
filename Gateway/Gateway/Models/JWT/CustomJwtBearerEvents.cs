using Clients.AuthClientService;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CustomGateway.Models.JWT
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        private readonly ITokenValidationClient _tokenValidationClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomJwtBearerEvents(ITokenValidationClient authClientService, IHttpContextAccessor httpContextAccessor)
        {
            _tokenValidationClient = authClientService;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            var token = context.Request.Headers.Authorization.ToString().Split(' ')[1];

            var result = await _tokenValidationClient.IsTokenValid(token);

            if (!result)
            {
                context.Fail("Token is banned by auth service");
                if (_httpContextAccessor.HttpContext != null)
                {
                    _httpContextAccessor.HttpContext.Request.Headers.Authorization = new();
                }

                return;
            }

            await base.TokenValidated(context);
        }
    }
}
