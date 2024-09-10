using Application.DTOs.User;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class UpdateNotSensitiveUserInfoComand : IRequest<Response<string>>
    {
        public UpdateNotSensetiveInfoDto UpdateNotSensetiveInfoDto { get; set; } = null!;
    }
}
