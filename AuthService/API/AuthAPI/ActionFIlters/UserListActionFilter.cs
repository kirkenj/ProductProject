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
                HadnleUserListDtoCollection(userList);
            }


            if (objectResult.Value is UserDto userDto)
            {
                HadnleSingleUserDto(userDto);
            }

            await next();
        }

        private static void HadnleSingleUserDto(UserDto userDto)
        {
            SetDefaultValues(userDto);
        }

        private static void HadnleUserListDtoCollection(IReadOnlyCollection<UserDto> userList)
        {
            foreach (var item in userList)
            {
                if (item != null)
                {
                    SetDefaultValues(item);
                }
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
