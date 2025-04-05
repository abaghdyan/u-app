using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VistaLOS.Application.Common.Enums;
using VistaLOS.Application.Common.Exceptions;
using VistaLOS.Application.Identity.Data.Entities;
using VistaLOS.Application.Identity.Data.Repositories;
using VistaLOS.Application.Identity.Services.Models.Role;

namespace VistaLOS.Application.Identity.Services.Managers;

public class IdentityRoleManager
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public IdentityRoleManager(IMapper mapper,
        IRoleRepository roleRepository)
    {
        _mapper = mapper;
        _roleRepository = roleRepository;
    }

    public async Task<List<RoleModel>> GetRolesAsync()
    {
        var roleEntities = await _roleRepository.Get()
            .ToListAsync();

        var roleDtos = _mapper.Map<List<RoleModel>>(roleEntities);

        return roleDtos;
    }

    public async Task<RoleModel?> GetRoleByIdAsync(long roleId)
    {
        var roleEntity = await _roleRepository.Get()
            .FirstOrDefaultAsync(u => u.Id == roleId);

        if (roleEntity == null) {
            return null;
        }

        var roleDto = _mapper.Map<RoleModel>(roleEntity);

        return roleDto;
    }
    public async Task<int> CreateRoleAsync(RoleCreateModel role)
    {
        var roleEntity = _mapper.Map<RoleEntity>(role);

        return await _roleRepository.CreateAsync(roleEntity);
    }

    public async Task<int> UpdateRoleAsync(RoleUpdateModel role)
    {
        var roleEntity = await _roleRepository.Get()
            .FirstOrDefaultAsync(u => u.Id == role.Id);

        if (roleEntity == null) {
            throw new ClientFacingException("Role not found", ErrorTypes.NotFound);
        }

        roleEntity.Name = role.Name;
        roleEntity.Description = role.Description;

        return await _roleRepository.UpdateAsync(roleEntity);
    }
}