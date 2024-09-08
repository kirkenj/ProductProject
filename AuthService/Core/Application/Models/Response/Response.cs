using FluentValidation.Results;
using System.Net;

namespace Application.Models.Response
{
    public class Response<T>
    {
        public string Message = null!;
        public T? Result;
        public HttpStatusCode StatusCode;
        public bool Success = false;


        public static Response<T> BadRequestResponse(string message) => new()
        {
            Message = message,
            Success = false,
            StatusCode = HttpStatusCode.BadRequest,
            Result = default
        };

        public static Response<T> BadRequestResponse(IEnumerable<ValidationFailure> validationFailures) => new()
        {
            Message = string.Join(";", validationFailures.Select(e => e.ErrorMessage)),
            Success = false,
            StatusCode = HttpStatusCode.BadRequest,
            Result = default
        };

        public static Response<T> ServerErrorResponse(string message) => new()
        {
            Message = message,
            Success = false,
            StatusCode = HttpStatusCode.InternalServerError,
            Result = default
        };

        public static Response<T> NotFoundResponse(string message, bool messageIsArgumentName = false) => new()
        {
            Success = false,
            Message = messageIsArgumentName ? $"Object with given {message} not found" : message,
            StatusCode = HttpStatusCode.NotFound,
            Result = default
        };

        public static Response<T> OkResponse(T result, string message) => new()
        {
            Success = true,
            Message = message,
            Result = result,
            StatusCode = HttpStatusCode.OK
        };
    }
}
