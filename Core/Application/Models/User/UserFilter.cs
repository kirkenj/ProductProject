namespace Application.Models.User
{
    public class UserFilter
    {
        public IEnumerable<Guid>? Ids { get; set; }
        public string? AccurateLogin { get; set; }
        public string? LoginPart { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int? Role { get; set; }
        public bool? EmailConfirmed { get; set; }
    }
}
