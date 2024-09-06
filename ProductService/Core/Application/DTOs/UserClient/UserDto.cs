namespace Application.DTOs.UserClient
{
    public class UserDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("login")]
        public string Login { get; set; } = null!;

        [System.Text.Json.Serialization.JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [System.Text.Json.Serialization.JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [System.Text.Json.Serialization.JsonPropertyName("address")]
        public string Address { get; set; } = null!;

        [System.Text.Json.Serialization.JsonPropertyName("role")]
        public RoleDto Role { get; set; } = null!;
    }
}
