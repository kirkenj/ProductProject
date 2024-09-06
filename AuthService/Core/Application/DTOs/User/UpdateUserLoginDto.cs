namespace Application.DTOs.User
{
    public class UpdateUserLoginDto
    {
        public Guid Id { get; set; }
        public string NewLogin { get; set; } = null!;
    }
}