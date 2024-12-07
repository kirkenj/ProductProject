using Clients.AuthApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomGateway.Controllers;

public class AuthServiceController : Microsoft.AspNetCore.Mvc.ControllerBase
{
    private readonly IAuthApiClient _implementation = null!;

    public AuthServiceController(IAuthApiClient implementation)
    {
        _implementation = implementation;
    }


    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Account")]
    public System.Threading.Tasks.Task<UserDto> AccountGET()
    {
        return _implementation.AccountGETAsync();
    }


    [Authorize]
    [Produces("text/plain")]
    [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Account")]
    public System.Threading.Tasks.Task<string> AccountPUT([Microsoft.AspNetCore.Mvc.FromBody] UpdateUserModel body)
    {
        return _implementation.AccountPUTAsync(body);
    }


    [Authorize]
    [Produces("text/plain")]
    [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Account/Password")]
    public System.Threading.Tasks.Task<string> Password([Microsoft.AspNetCore.Mvc.FromBody] string body)
    {
        return _implementation.PasswordAsync(body);
    }


    [Authorize]
    [Produces("text/plain")]
    [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Account/Login")]
    public System.Threading.Tasks.Task<string> LoginPUT([Microsoft.AspNetCore.Mvc.FromQuery] string newLogin)
    {
        return _implementation.LoginPUTAsync(newLogin);
    }


    [Authorize]
    [Produces("text/plain")]
    [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Account/Email")]
    public System.Threading.Tasks.Task<string> Email([Microsoft.AspNetCore.Mvc.FromBody] string body)
    {
        return _implementation.EmailAsync(body);
    }


    [Authorize]
    [Produces("text/plain")]
    [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/Account/Email/Confirm")]
    public System.Threading.Tasks.Task<string> Confirm([Microsoft.AspNetCore.Mvc.FromBody] string body)
    {
        return _implementation.ConfirmAsync(body);
    }


    [Produces("text/plain")]
    [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/Auth/Register")]
    public System.Threading.Tasks.Task<string> Register([Microsoft.AspNetCore.Mvc.FromBody] CreateUserDto body)
    {
        return _implementation.RegisterAsync(body);
    }


    [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/Auth/Login")]
    public System.Threading.Tasks.Task<LoginResultModel> LoginPOST([Microsoft.AspNetCore.Mvc.FromBody] LoginDto body)
    {
        return _implementation.LoginPOSTAsync(body);
    }


    [Produces("text/plain")]
    [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/Auth/ForgotPassword")]
    public System.Threading.Tasks.Task<string> ForgotPassword([Microsoft.AspNetCore.Mvc.FromBody] string body)
    {
        return _implementation.ForgotPasswordAsync(body);
    }


    [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Roles")]
    public System.Threading.Tasks.Task<System.Collections.Generic.ICollection<RoleDto>> RolesAll()
    {
        return _implementation.RolesAllAsync();
    }


    [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Roles/{id}")]
    public System.Threading.Tasks.Task<RoleDto> Roles(int id)
    {
        return _implementation.RolesAsync(id);
    }


    [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Tokens/IsValid")]
    public System.Threading.Tasks.Task<bool> IsValid([Microsoft.AspNetCore.Mvc.FromQuery] string tokenHash)
    {
        return _implementation.IsValidAsync(tokenHash);
    }


    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/Tokens/TerminateSessions")]
    public System.Threading.Tasks.Task TerminateSessions([Microsoft.AspNetCore.Mvc.FromQuery] System.Guid? userId)
    {
        return _implementation.TerminateSessionsAsync(userId);
    }


    [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Tokens/HashProviderSettings")]
    public System.Threading.Tasks.Task<HashProviderSettings> HashProviderSettings()
    {
        return _implementation.HashProviderSettingsAsync();
    }


    [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Users/list")]
    public System.Threading.Tasks.Task<System.Collections.Generic.ICollection<UserDto>> List([Microsoft.AspNetCore.Mvc.FromQuery] System.Collections.Generic.IEnumerable<System.Guid> ids, [Microsoft.AspNetCore.Mvc.FromQuery] string accurateLogin, [Microsoft.AspNetCore.Mvc.FromQuery] string loginPart, [Microsoft.AspNetCore.Mvc.FromQuery] string accurateEmail, [Microsoft.AspNetCore.Mvc.FromQuery] string emailPart, [Microsoft.AspNetCore.Mvc.FromQuery] string address, [Microsoft.AspNetCore.Mvc.FromQuery] string name, [Microsoft.AspNetCore.Mvc.FromQuery] System.Collections.Generic.IEnumerable<int> roleIds, [Microsoft.AspNetCore.Mvc.FromQuery] int? page, [Microsoft.AspNetCore.Mvc.FromQuery] int? pageSize)
    {
        return _implementation.ListAsync(ids, accurateLogin, loginPart, accurateEmail, emailPart, address, name, roleIds, page, pageSize);
    }


    [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Users/{id}")]
    public System.Threading.Tasks.Task<UserDto> UsersGET(System.Guid id)
    {
        return _implementation.UsersGETAsync(id);
    }


    [Authorize]
    [Produces("text/plain")]
    [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Users/{id}")]
    public System.Threading.Tasks.Task<string> UsersPUT(System.Guid id, [Microsoft.AspNetCore.Mvc.FromBody] UpdateUserModel body)
    {
        return _implementation.UsersPUTAsync(id, body);
    }


    [Authorize]
    [Produces("text/plain")]
    [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Users/{id}/Email")]
    public System.Threading.Tasks.Task<string> Email2(System.Guid id, [Microsoft.AspNetCore.Mvc.FromBody] string body)
    {
        return _implementation.Email2Async(id, body);
    }


    [Produces("text/plain")]
    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/Users/{id}/Email/Confirm")]
    public System.Threading.Tasks.Task<string> Confirm2(System.Guid id, [Microsoft.AspNetCore.Mvc.FromBody] string body)
    {
        return _implementation.Confirm2Async(id, body);
    }


    [Produces("text/plain")]
    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Users/{id}/Login")]
    public System.Threading.Tasks.Task<string> LoginPUT2(System.Guid id, [Microsoft.AspNetCore.Mvc.FromQuery] string newLogin)
    {
        return _implementation.LoginPUT2Async(id, newLogin);
    }


    [Authorize]
    [Produces("text/plain")]
    [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Users/{id}/Role")]
    public System.Threading.Tasks.Task<string> Role(System.Guid id, [Microsoft.AspNetCore.Mvc.FromQuery] int? roleId)
    {
        return _implementation.RoleAsync(id, roleId);
    }
}