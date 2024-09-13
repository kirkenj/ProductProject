namespace Application.Models.User
{
    public class CreateUserSettings
    {
        public int DefaultRoleID { get; set; }
        public double EmailConfirmationTimeoutHours { get; set; }
    }
}
