using AuthAPI.Extensions;
using Infrastructure.TockenTractker;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly TokenTracker _tokenTracker;

        public TokensController(IMediator mediator, TokenTracker tokenTracker)
        {
            _mediator = mediator;
            _tokenTracker = tokenTracker;
        }

        [HttpGet("IsTokenValid")]
        public async Task<ActionResult<bool>> Get(string token)
        {
            return await Task.FromResult(Ok(_tokenTracker.IsValid(token)));
        }

        [HttpPost("InvalidateUsersToken")]
        public async Task<ActionResult> InvalidateToken(Guid userId)
        {
            _tokenTracker.InvalidateUser(userId, DateTime.UtcNow); 

            return await Task.FromResult(Ok());
        }
    }
}
