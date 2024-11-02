using Clients.Contracts;
using Clients.CustomGateway;
using Front.Services;

namespace Front.Models
{
    public class TokenGetter : ITokenGetter<IGatewayClient>
    {
        private readonly UserService _userService;

        public TokenGetter(UserService userService)
        {
            _userService = userService;
        }

        public async Task<string?> GetToken()
        {
            return await _userService.GetAuthTokenAsync();
        }
    }
}
