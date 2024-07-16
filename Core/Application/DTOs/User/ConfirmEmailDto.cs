namespace Application.DTOs.User
{
    public class ConfirmEmailDto
    {
        public Guid UserId { get; set; }
        public string Key { get; set; } = null!;
    }
}
