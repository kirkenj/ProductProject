//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

#nullable enable

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 612 // Disable "CS0612 '...' is obsolete"
#pragma warning disable 649 // Disable "CS0649 Field is never assigned to, and will always have its default value null"
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"
#pragma warning disable 8604 // Disable "CS8604 Possible null reference argument for parameter"
#pragma warning disable 8625 // Disable "CS8625 Cannot convert null literal to non-nullable reference type"
#pragma warning disable 8765 // Disable "CS8765 Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes)."

namespace CustomGateway.Controllers.Auth
{
    using System = global::System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using global::Clients.AuthApi;

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    [ApiController]

    public partial class AuthServiceController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private IAuthApiClient _implementation;

        public AuthServiceController(IAuthApiClient implementation)
        {
            _implementation = implementation;
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Account/TokenClaims", Name = "TokenClaims")]
        [Authorize]
        public async System.Threading.Tasks.Task<System.Collections.Generic.ICollection<StringStringKeyValuePair>> TokenClaims()
        {

            return await _implementation.TokenClaimsAsync();
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Account", Name = "AccountGET")]
        [Authorize]
        public async System.Threading.Tasks.Task<UserDto> AccountGET()
        {

            return await _implementation.AccountGETAsync();
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Account", Name = "AccountPUT")]
        [Authorize]
        [Produces("text/plain")]
        public async System.Threading.Tasks.Task<string> AccountPUT([Microsoft.AspNetCore.Mvc.FromBody] UpdateUserModel? body)
        {

            return await _implementation.AccountPUTAsync(body);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Account/Password", Name = "Password")]
        [Authorize]
        [Produces("text/plain")]
        public async System.Threading.Tasks.Task<string> Password([FromBody] string newPassword)
        {

            return await _implementation.PasswordAsync(newPassword);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Account/UserTag", Name = "UserTag")]
        [Authorize]
        [Produces("text/plain")]
        public async System.Threading.Tasks.Task<string> UserTag([Microsoft.AspNetCore.Mvc.FromQuery] string? newLogin)
        {

            return await _implementation.UserTagAsync(newLogin);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Account/Email", Name = "EmailPUT")]
        [Authorize]
        [Produces("text/plain")]
        public async System.Threading.Tasks.Task<string> EmailPUT([Microsoft.AspNetCore.Mvc.FromBody] string? body)
        {

            return await _implementation.EmailPUTAsync(body);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/Account/Email", Name = "EmailPOST")]
        [Authorize]
        [Produces("text/plain")]
        public async System.Threading.Tasks.Task<string> EmailPOST([Microsoft.AspNetCore.Mvc.FromBody] string? body)
        {

            return await _implementation.EmailPOSTAsync(body);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/Auth/Register", Name = "Register")]
        public async System.Threading.Tasks.Task<System.Guid> Register([Microsoft.AspNetCore.Mvc.FromBody] CreateUserDto? body)
        {

            return await _implementation.RegisterAsync(body);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/Auth/Login", Name = "Login")]
        public async System.Threading.Tasks.Task<LoginResultModel> Login([Microsoft.AspNetCore.Mvc.FromBody] LoginDto? body)
        {
            return await _implementation.LoginAsync(body);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/Auth/ForgotPassword", Name = "ForgotPassword")]
        [Produces("text/plain")]
        public async System.Threading.Tasks.Task<string> ForgotPassword([Microsoft.AspNetCore.Mvc.FromBody] string? body)
        {
            var q = await _implementation.ForgotPasswordAsync(body);
            return q;
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Roles/Roles", Name = "RolesAll")]
        public async System.Threading.Tasks.Task<System.Collections.Generic.ICollection<RoleDto>> RolesAll()
        {

            return await _implementation.RolesAllAsync();
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Roles/Roles/{id}", Name = "Roles")]
        public async System.Threading.Tasks.Task<RoleDto> Roles(int id)
        {

            return await _implementation.RolesAsync(id);
        }


        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/Tokens/InvalidateUsersToken", Name = "InvalidateUsersToken")]
        [Authorize]
        public async System.Threading.Tasks.Task InvalidateUsersToken([Microsoft.AspNetCore.Mvc.FromQuery] System.Guid? userId)
        {
            await _implementation.InvalidateUsersTokenAsync(userId);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Users/list", Name = "list")]
        public async System.Threading.Tasks.Task<System.Collections.Generic.ICollection<UserDto>> List([Microsoft.AspNetCore.Mvc.FromQuery] System.Collections.Generic.IEnumerable<System.Guid>? ids, [Microsoft.AspNetCore.Mvc.FromQuery] string? accurateLogin, [Microsoft.AspNetCore.Mvc.FromQuery] string? loginPart, [Microsoft.AspNetCore.Mvc.FromQuery] string? accurateEmail, [Microsoft.AspNetCore.Mvc.FromQuery] string? emailPart, [Microsoft.AspNetCore.Mvc.FromQuery] string? address, [Microsoft.AspNetCore.Mvc.FromQuery] string? name, [Microsoft.AspNetCore.Mvc.FromQuery] System.Collections.Generic.IEnumerable<int>? roleIds, [Microsoft.AspNetCore.Mvc.FromQuery] int? page, [Microsoft.AspNetCore.Mvc.FromQuery] int? pageSize)
        {
            return await _implementation.ListAsync(ids, accurateLogin, loginPart, accurateEmail, emailPart, address, name, roleIds, page, pageSize);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Users/{id}", Name = "UsersGET")]
        public async System.Threading.Tasks.Task<UserDto> UsersGET(System.Guid id)
        {

            return await _implementation.UsersGETAsync(id);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Users", Name = "UsersPUT")]
        [Authorize]
        [Produces("text/plain")]
        public async System.Threading.Tasks.Task<string> UsersPUT([Microsoft.AspNetCore.Mvc.FromBody] UpdateUserInfoDto? body)
        {

            return await _implementation.UsersPUTAsync(body);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Users/Email", Name = "EmailPUT2")]
        [Authorize]
        [Produces("text/plain")]
        public async System.Threading.Tasks.Task<string> EmailPUT2([Microsoft.AspNetCore.Mvc.FromBody] UpdateUserEmailDto? body)
        {

            return await _implementation.EmailPUT2Async(body);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/Users/Email", Name = "EmailPOST2")]
        [Authorize]
        [Produces("text/plain")]
        public async System.Threading.Tasks.Task<string> EmailPOST2([Microsoft.AspNetCore.Mvc.FromBody] ConfirmEmailChangeDto? body)
        {
            return await _implementation.EmailPOST2Async(body);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Users/UserTag", Name = "UserTag2")]
        [Authorize]
        [Produces("text/plain")]
        public async System.Threading.Tasks.Task<string> UserTag2([Microsoft.AspNetCore.Mvc.FromBody] UpdateUserLoginDto? body)
        {
            return await _implementation.UserTag2Async(body);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Users/Role", Name = "Role")]
        [Authorize]
        [Produces("text/plain")]
        public async System.Threading.Tasks.Task<string> Role([Microsoft.AspNetCore.Mvc.FromBody] UpdateUserRoleDTO? body)
        {
            return await _implementation.RoleAsync(body);
        }
    }
}

#pragma warning restore 108
#pragma warning restore 114
#pragma warning restore 472
#pragma warning restore 612
#pragma warning restore 1573
#pragma warning restore 1591
#pragma warning restore 8073
#pragma warning restore 3016
#pragma warning restore 8603
#pragma warning restore 8604
#pragma warning restore 8625