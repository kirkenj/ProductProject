using Application.DTOs.User;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class UpdateUserAddressComand : IRequest
    {
        public UpdateUserAddressDto UpdateUserAddressDto { get; set; } = null!;
    }
}
