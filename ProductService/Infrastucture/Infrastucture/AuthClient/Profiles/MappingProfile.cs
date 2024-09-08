using Application.DTOs.Product;
using AutoMapper;
using Domain.Models;

namespace Infrastructure.AuthClient.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Application.DTOs.UserClient.GetHashDefaultsResponse, Clients.AuthClientService.GetHashDefaultsResponce>().ReverseMap();
            CreateMap<Application.DTOs.UserClient.UserListDto, Clients.AuthClientService.UserListDto>().ReverseMap();
            CreateMap<Application.DTOs.UserClient.RoleDto, Clients.AuthClientService.RoleDto>().ReverseMap();
            CreateMap<Application.DTOs.UserClient.UserDto, Clients.AuthClientService.UserDto>().ReverseMap();
        }
    }
}
