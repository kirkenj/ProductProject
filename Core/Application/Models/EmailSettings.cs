namespace Application.Models
{
    public class EmailSettings
    {
        public string FromName { get; set; } = null!;
        public string FromAdress { get; set; } = null!;
        public int FromPort { get; set; }
        public string ApiLogin { get; set; } = null!;
        public string ApiPassword { get; set; } = null!;
    }
}
