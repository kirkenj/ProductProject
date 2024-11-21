using Application.DTOs.Role;
using Application.DTOs.User;
using Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthAPI.FIlters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GetUserActionFilterAttribute : Attribute, IAsyncResultFilter
    {
        public const string ADDRESS_PLACEHOLDER = "Contact administration to get this information";
        public static RoleDto DefaultRole => new() { Id = 2, Name = ApiConstants.REGULAR_ROLE_NAME };

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is not ObjectResult objectResult)
            {
                context.Cancel = true;
                return;
            }

            if (context.HttpContext.User.IsInRole(ApiConstants.ADMIN_ROLE_NAME))
            {
                await next();
                return;
            }

            if (objectResult.Value is IReadOnlyCollection<UserDto> userList)
            {
                SetDefaultValues(userList);
            }
            else if (objectResult.Value is UserDto userDto)
            {
                SetDefaultValues(userDto);
            }

            await next();
        }

        private static void SetDefaultValues(IReadOnlyCollection<UserDto> userList)
        {
            foreach (var item in userList)
            {
                SetDefaultValues(item);
            }
        }

        private static UserDto SetDefaultValues(UserDto user)
        {
            user.Role = DefaultRole;
            user.Address = ADDRESS_PLACEHOLDER;
            return user;
        }
    }
}
