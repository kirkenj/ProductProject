using Application.Contracts.Infrastructure;
using Infrastructure.HashProvider;
using AuthAPI.Contracts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly ITokenTracker<Guid> _tokenTracker;
        private readonly IHashProvider _hashProvider;

        public TokensController(ITokenTracker<Guid> tokenTracker, IHashProvider hashProvider)
        {
            _hashProvider = hashProvider;
            _tokenTracker = tokenTracker;
        }

        [HttpPost("IsTokenValid")]
        public async Task<ActionResult<bool>> IsTokenValid([FromBody] string tokenHash)
        {
            bool result = await _tokenTracker.IsValid(tokenHash);
            return Ok(result);
        }

        [HttpPost("InvalidateUsersToken")]
        public async Task<ActionResult> InvalidateToken(Guid userId)
        {
            await _tokenTracker.InvalidateUser(userId, DateTime.UtcNow);
            return Ok();
        }

        [HttpGet("GetHashDefaults")]
        public HashProviderSettings GetHashDefaults() => new() { HashAlgorithmName = _tokenTracker.HashProvider.HashAlgorithmName, EncodingName = _tokenTracker.HashProvider.Encoding.BodyName };
    }
}
