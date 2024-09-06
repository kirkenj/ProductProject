namespace Application.Models.Email
{
    public class EmailUpdateDetails
    {
        public Guid UserId { get; set; }
        public string NewEmail { get; set; } = null!;
    }
}
