using Application.Contracts.Infrastructure;
using AuthAPI.Extensions;
using AuthAPI.Models;
using Infrastructure.TockenTractker;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly TokenTracker<Guid> _tokenTracker;
        private readonly IHashProvider _hashProvider;

        public TokensController(TokenTracker<Guid> tokenTracker, IHashProvider hashProvider)
        {
            _hashProvider = hashProvider;
            _tokenTracker = tokenTracker;
        }

        [HttpPost("IsTokenValid")]
        public async Task<ActionResult<bool>> IsTokenValid([FromBody] string tokenHash)
        {
            return await Task.FromResult(Ok(_tokenTracker.IsValid(tokenHash)));
        }

        [HttpGet("IsTokenValid")]
        public async Task<ActionResult<bool>> IsTokenValidGet(string token)
        {
            return await Task.FromResult(Ok(_tokenTracker.IsValid(_hashProvider.GetHash(token))));
        }

        [HttpPost("InvalidateUsersToken")]
        public async Task<ActionResult> InvalidateToken(Guid userId)
        {
            _tokenTracker.InvalidateUser(userId, DateTime.UtcNow);

            return await Task.FromResult(Ok());
        }

        [HttpGet("GetHashDefaults")]
        public GetHashDefaultsResponce GetHashDefaults() => new() { HashAlgorithmName = _tokenTracker.HashProvider.HashAlgorithmName, EncodingName = _tokenTracker.HashProvider.Encoding.BodyName };
    }
}
