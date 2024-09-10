using AutoMapper;

namespace HashProvider.AuthClient.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Application.DTOs.UserClient.GetHashDefaultsResponse, Clients.AuthApi.GetHashDefaultsResponce>().ReverseMap();
            CreateMap<Application.DTOs.UserClient.UserListDto, Clients.AuthApi.UserListDto>().ReverseMap();
            CreateMap<Application.DTOs.UserClient.RoleDto, Clients.AuthApi.RoleDto>().ReverseMap();
            CreateMap<Application.DTOs.UserClient.UserDto, Clients.AuthApi.UserDto>().ReverseMap();
        }
    }
}
