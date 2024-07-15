namespace Application.DTOs.User
{
    public class UpdateUserEmailConfirmStatusDto
    {
        public string Email { get; set; } = null!;
        public bool Status { get; set; }
    }
}