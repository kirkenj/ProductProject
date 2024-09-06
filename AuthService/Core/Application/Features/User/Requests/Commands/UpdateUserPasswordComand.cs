using Application.DTOs.User;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class UpdateUserPasswordComand : IRequest<Response<string>>
    {
        public UpdateUserPasswordDto UpdateUserPasswordDto { get; set; } = null!;
    }
}
