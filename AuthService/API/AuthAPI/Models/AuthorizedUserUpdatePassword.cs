namespace AuthAPI.Models
{
    public class AuthorizedUserUpdatePassword
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
