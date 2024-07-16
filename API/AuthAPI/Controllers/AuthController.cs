using Application.DTOs.Role;
using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using Application.Features.User.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _memoryCache;


        public AuthController(IMediator mediator, IMemoryCache memoryCache)
        {
            _mediator = mediator;
            _memoryCache = memoryCache;
        }
        

        // POST api/<AuthController2>
        [HttpPost("Register")]
        public async Task<Guid> Post([FromBody] CreateUserDto createUserDto)
        {
            var result = await _mediator.Send(new CreateUserCommand() { CreateUserDto = createUserDto });
            return result;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto) 
        {
            var result = await _mediator.Send(loginDto);
            if (result == null)
            {
                return false.ToString();
            }

            return result;
        }

        [HttpPost("Cache")]
        public void AddCache(string key, string value)
        {
            _memoryCache.Set(key, value, DateTimeOffset.Now.AddSeconds(10));
        }

        [HttpGet("Cache")]
        public string GetCache(string? key) 
        { 
            if (string.IsNullOrEmpty(key))
            {
                return _memoryCache.GetCurrentStatistics().CurrentEntryCount.ToString();
            }

            return _memoryCache.Get(key)?.ToString() ?? "Not found";
        }

    }
}
