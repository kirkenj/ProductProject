using Application.DTOs.User;

namespace AuthAPI.Contracts
{
    public interface IJwtProviderService
    {
        public string GetToken(UserDto user);
    }
}
