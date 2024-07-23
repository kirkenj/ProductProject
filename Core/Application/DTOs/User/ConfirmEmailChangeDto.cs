namespace Application.DTOs.User
{
    public class ConfirmEmailChangeDto
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;
    }
}
