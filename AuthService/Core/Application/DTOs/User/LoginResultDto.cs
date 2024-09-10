namespace Application.DTOs.User
{
    public class LoginResultDto
    {
        public string Token { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}
