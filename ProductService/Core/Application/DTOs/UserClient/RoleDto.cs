namespace Application.DTOs.UserClient
{
    public class RoleDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public int Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("name")]
        public string Name { get; set; } = null!;

    }
}
