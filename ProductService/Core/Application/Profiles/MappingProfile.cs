using Application.DTOs.Product;
using Application.Models.UserClient;
using AutoMapper;
using Clients.AuthApi;
using Domain.Models;

namespace Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductListDto>();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<UserDto, AuthClientUser>().ReverseMap();
            CreateMap<RoleDto, AuthClientRole>().ReverseMap();
        }
    }
}
