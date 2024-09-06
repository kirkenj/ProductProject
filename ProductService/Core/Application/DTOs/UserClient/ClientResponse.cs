namespace Application.DTOs.UserClient
{
    public class ClientResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public T Result { get; set; } = default!;
    }
}
