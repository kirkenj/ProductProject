using Application.DTOs.Role;
using Application.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthAPI.FIlters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GetUserActionFilterAttribute : Attribute, IAsyncResultFilter
    {
        public static RoleDto DefaultRole => new() { Id = 2, Name = Constants.Constants.REGULAR_ROLE_NAME };

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is not ObjectResult objectResult)
            {
                context.Cancel = true;
                return;
            }

            if (context.HttpContext.User.IsInRole(Constants.Constants.ADMIN_ROLE_NAME))
            {
                await next();
                return;
            }

            if (objectResult.Value is IReadOnlyCollection<UserListDto> userList)
            {
                HadnleUserListDtoCollection(userList);
            }


            if (objectResult.Value is UserDto userDto)
            {
                HadnleSingleUserDto(userDto);
            }

            await next();
        }

        private void HadnleSingleUserDto(UserDto userDto)
        {
            SetDefaultValues(userDto);
        }

        private void HadnleUserListDtoCollection(IReadOnlyCollection<UserListDto> userList)
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
            user.Address = Constants.Constants.ADDRESS_PLACEHOLDER;
            return user;
        }

        private static UserListDto SetDefaultValues(UserListDto user)
        {
            user.Role = DefaultRole;
            return user;
        }
    }
}
