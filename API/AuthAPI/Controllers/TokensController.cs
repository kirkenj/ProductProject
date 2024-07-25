﻿using Application.Features.Tokens.Requests.Commands;
using Application.Features.Tokens.Requests.Queries;
using AuthAPI.Extensions;
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

        public TokensController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("IsTokenValid")]
        public async Task<ActionResult<bool>> Get(string token)
        {
            var result = await _mediator.Send(new IsTokenValidRequest { IsTokenValidDto = new() { Token = token } });
            return result.GetActionResult();
        }

        [HttpPost("InvalidateUsersToken")]
        public async Task<ActionResult> InvalidateToken(Guid userId)
        {
            var result = await _mediator.Send(new InvalidateUserTokensCommand
            {
                InvalidateUserTokensDto = new()
                {
                    AssigedTokenInfo = new() 
                    { 
                        UserId = userId, 
                        DateTime = DateTime.UtcNow 
                    }
                }
            });
            return result.GetActionResult();
        }
    }
}
