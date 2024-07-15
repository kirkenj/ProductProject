using Application.DTOs.User;

namespace Application.Contracts.Infrastructure
{
    public interface IJwtService
    {
        public string GetToken(UserDto user);
    }
}
