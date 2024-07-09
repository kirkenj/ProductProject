namespace Application.DTOs.User
{
    public class UpdateUserEmailConfirmStatusDTO
    {
        public string Email { get; set; } = null!;
        public bool Status { get; set; }
    }
}