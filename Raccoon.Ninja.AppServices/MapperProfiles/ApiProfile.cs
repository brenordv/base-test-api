using AutoMapper;
using Raccoon.Ninja.Domain.Entities;
using Raccoon.Ninja.Domain.Models;

namespace Raccoon.Ninja.AppServices.MapperProfiles;

public class ApiProfile: Profile
{
    public ApiProfile()
    {
        CreateMap<Product, AddProductModel>().ReverseMap();
        CreateMap<Product, ProductModel>().ReverseMap();
        CreateMap<User, UserModel>().ReverseMap();
    }
}