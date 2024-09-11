using Application.DTOs.User;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class UpdateNotSensitiveUserInfoComand : IRequest<Response<string>>
    {
        public UpdateUserInfoDto UpdateNotSensetiveInfoDto { get; set; } = null!;
    }
}
