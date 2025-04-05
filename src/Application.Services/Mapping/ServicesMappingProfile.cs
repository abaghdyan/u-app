using AutoMapper;
using VistaLOS.Application.Identity.Services.Models;
using VistaLOS.Application.Identity.Services.Models.User;
using VistaLOS.Application.Services.Models.UserManagement.User;

namespace VistaLOS.Application.Api.Mapping;

public class ServicesMappingProfile : Profile
{
    public ServicesMappingProfile()
    {
        MapIdentity();
    }

    private void MapIdentity()
    {
        CreateMap<UserUpdateDto, UserUpdateModel>();
        CreateMap<UserCreateDto, UserCreateModel>();
        CreateMap<UserModel, UserDto>();
    }
}
