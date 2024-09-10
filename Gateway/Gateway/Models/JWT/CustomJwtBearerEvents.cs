using Clients.AuthApi.AuthApiIStokenValidClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CustomGateway.Models.JWT
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        private readonly ITokenValidationClient _tokenValidationClient;

        public CustomJwtBearerEvents(ITokenValidationClient authClientService)
        {
            _tokenValidationClient = authClientService;
        }

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            var token = context.Request.Headers.Authorization.ToString().Split(' ')[1];

            var result = await _tokenValidationClient.IsTokenValid(token);

            if (!result)
            {
                context.Fail("Token is banned by auth service");
            }

            await base.TokenValidated(context);
        }
    }
}
