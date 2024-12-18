﻿using AuthAPI.Contracts;
using Extensions.ClaimsPrincipalExtensions;
using HashProvider.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly ITokenTracker<Guid> _tokenTracker;

        public TokensController(ITokenTracker<Guid> tokenTracker)
        {
            _tokenTracker = tokenTracker;
        }

        [HttpGet("IsValid")]
        public async Task<ActionResult<bool>> IsTokenValid(string tokenHash)
        {
            bool result = await _tokenTracker.IsValid(tokenHash);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("TerminateSessions")]
        public async Task<ActionResult> InvalidateToken(Guid userId)
        {
            if (!User.IsInRole(Constants.ApiConstants.ADMIN_ROLE_NAME) && User.GetUserId() != userId)
            {
                return Forbid();
            }

            await _tokenTracker.InvalidateUser(userId, DateTime.UtcNow);
            return Ok();
        }

        [HttpGet("HashProviderSettings")]
        public HashProviderSettings GetHashDefaults() => new() { HashAlgorithmName = _tokenTracker.HashProvider.HashAlgorithmName, EncodingName = _tokenTracker.HashProvider.Encoding.BodyName };
    }
}
