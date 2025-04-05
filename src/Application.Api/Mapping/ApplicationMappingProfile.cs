using Application.ExternalContracts.UserManagement;
using AutoMapper;
using VistaLOS.Application.Identity.Services.Models;
using VistaLOS.Application.Services.Models.UserManagement.User;

namespace VistaLOS.Application.Api.Mapping;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        MapIdentity();
    }

    private void MapIdentity()
    {
        CreateMap<AccessTokenModel, AccessTokenVm>();
        CreateMap<UserSignInIm, SignInModel>();

        CreateMap<UserUpdateIm, UserUpdateDto>();
        CreateMap<UserCreateIm, UserCreateDto>();
        CreateMap<UserDto, UserVm>();
    }
}
