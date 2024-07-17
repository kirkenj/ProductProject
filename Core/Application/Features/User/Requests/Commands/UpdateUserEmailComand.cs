using Application.DTOs.User;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class UpdateUserEmailComand : IRequest<Response<string>>
    {
        public UpdateUserEmailDto UpdateUserEmailDto { get; set; } = null!;
    }
}

