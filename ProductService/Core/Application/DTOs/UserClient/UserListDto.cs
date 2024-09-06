namespace Application.DTOs.UserClient
{
    public class UserListDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("login")]
        public string Login { get; set; } = null!;

        [System.Text.Json.Serialization.JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [System.Text.Json.Serialization.JsonPropertyName("role")]
        public RoleDto Role { get; set; } = null!;
    }
}
