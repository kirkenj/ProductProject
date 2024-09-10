using CustomResponse;
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

        public static ActionResult<Message> GetActionResultAsMessage(this Response<string> response) =>
            new ObjectResult(response.Success ? new Message { Title = response.Message, Body = response.Result ?? throw new Exception()} : response.Message)
            {
                StatusCode = (int)response.StatusCode
            };
    }

    public class Message
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
