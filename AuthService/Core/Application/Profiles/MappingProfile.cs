using Application.DTOs.Role;
using Application.DTOs.User;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserListDto>();
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, UpdateUserInfoDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
        }
    }
}
