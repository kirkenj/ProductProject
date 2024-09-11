using Application.DTOs.User.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class LoginDto : IEmailDto, IPasswordDto
    {
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
