using Application.DTOs.User.Interfaces;

namespace Application.DTOs.User
{
    public class ForgotPasswordDto : IEmailDto
    {
        public string Email { get; set; } = null!;
    }
}
