using Application.DTOs.User;

namespace Application.Contracts.Infrastructure
{
    public interface IJwtProviderService
    {
        public string GetToken(UserDto user);
    }
}
