using AutoMapper;
using VistaLOS.Application.Common.Extensions;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Identity.Services.Managers;
using VistaLOS.Application.Identity.Services.Models.User;
using VistaLOS.Application.Services.Abstractions;
using VistaLOS.Application.Services.Models.UserManagement.User;

namespace VistaLOS.Application.Services.Impl;

public class UserService : IUserService
{
    private readonly IdentityUserManager _identityUserManager;
    private readonly IUserContextAccessor _userContextAccessor;
    private readonly IMapper _mapper;

    public UserService(IdentityUserManager identityUserManager,
        IUserContextAccessor userContextAccessor,
        IMapper mapper)
    {
        _identityUserManager = identityUserManager;
        _userContextAccessor = userContextAccessor;
        _mapper = mapper;
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _identityUserManager.GetUsersAsync();
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto?> GetUserByIdAsync(long userId)
    {
        var user = await _identityUserManager.GetUserByIdAsync(userId);

        if (user == null) {
            return null;
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task AddUserAsync(UserCreateDto userCreateDto)
    {
        var userContext = _userContextAccessor.GetRequiredUserContext();

        var userToCreate = _mapper.Map<UserCreateModel>(userCreateDto);
        userToCreate.TenantId = userContext.TenantId;

        await _identityUserManager.CreateUserAsync(userToCreate);
    }

    public async Task UpdateUserAsync(UserUpdateDto userUpdateDto)
    {
        var userToUpdate = _mapper.Map<UserUpdateModel>(userUpdateDto);

        await _identityUserManager.UpdateUserAsync(userToUpdate);
    }

    public async Task UpdateUserImageAsync(long userId, string? image)
    {
        await _identityUserManager.UpdateUserImageAsync(userId, image);
    }
}
