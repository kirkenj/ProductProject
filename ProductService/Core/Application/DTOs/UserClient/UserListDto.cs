namespace Application.DTOs.UserClient
{
    public class UserListDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("login")]
        public string Login { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("email")]
        public string Email { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("role")]
        public RoleDto Role { get; set; } = default!;
    }
}
