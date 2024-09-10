using Application.DTOs.User;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class UpdateUserLoginComand : IRequest<Response<string>>
    {
        public UpdateUserLoginDto UpdateUserLoginDto { get; set; } = null!;
    }
}
