using Application.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Extensions
{
    public static class ResponseExtensions
    {
        public static ActionResult GetActionResult<T>(this Response<T> response) =>
            new ObjectResult(response.Success ? response.Result : response.Message)
            {
                StatusCode = (int)response.StatusCode
            };
    }
}
