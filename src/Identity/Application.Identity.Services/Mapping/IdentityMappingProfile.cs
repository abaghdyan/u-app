using AutoMapper;
using VistaLOS.Application.Common.Extensions;
using VistaLOS.Application.Identity.Data.Entities;
using VistaLOS.Application.Identity.Services.Models.Permissions;
using VistaLOS.Application.Identity.Services.Models.Role;
using VistaLOS.Application.Identity.Services.Models.User;

namespace VistaLOS.Application.Identity.Services.Mapping;

public class IdentityMappingProfile : Profile
{
    public IdentityMappingProfile()
    {
        MapUsers();

        MapRoles();

        MapPermissions();
    }

    private void MapUsers()
    {
        CreateMap<UserEntity, UserModel>()
            .ForMember(d => d.Role, c => c.MapFrom(e => e.Role));

        CreateMap<UserModel, UserUpdateModel>();

        CreateMap<UserCreateModel, UserEntity>()
            .ForMember(d => d.PasswordHash, c => c.MapFrom(e => e.Password.BuildSha256()));
    }

    private void MapRoles()
    {
        CreateMap<RoleEntity, RoleModel>()
            .ForMember(d => d.Permissions,
                c => c.MapFrom(e => e.RolePermissions.Select(rp => rp.Permission)));

        CreateMap<RoleCreateModel, RoleEntity>();
    }

    private void MapPermissions()
    {
        CreateMap<PermissionEntity, PermissionModel>();
    }
}
