namespace Application.Models.User
{
    public class LoginResult
    {
        public string Token { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}
