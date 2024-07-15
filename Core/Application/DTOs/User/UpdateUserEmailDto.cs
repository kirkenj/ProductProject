using MediatR;

namespace Application.DTOs.User
{
    public class UpdateUserEmailDto : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
    }
}
