namespace Application.DTOs.UserClient
{
    public class GetHashDefaultsResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("hashAlgorithmName")]
        public string HashAlgorithmName { get; set; } = null!;

        [System.Text.Json.Serialization.JsonPropertyName("encodingName")]
        public string EncodingName { get; set; } = null!;
    }
}
