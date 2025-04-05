using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VistaLOS.Application.Common.Enums;
using VistaLOS.Application.Common.Exceptions;
using VistaLOS.Application.Common.Extensions;
using VistaLOS.Application.Data.Master;
using VistaLOS.Application.Identity.Data.Entities;
using VistaLOS.Application.Identity.Data.Repositories;
using VistaLOS.Application.Identity.Services.Models.User;

namespace VistaLOS.Application.Identity.Services.Managers;

public class IdentityUserManager
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public IdentityUserManager(IMapper mapper,
        IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<bool> IsPasswordCorrectAsync(long userId, string password)
    {
        var passwordHash = password.BuildSha256();
        var user = await _userRepository.Get()
            .FirstOrDefaultAsync(u => u.Id == userId && u.PasswordHash == passwordHash);
        if (user != null) {
            return true;
        }
        return false;
    }

    public async Task SetNewPassword(long userId, string newPassword)
    {
        var user = await _userRepository.Get().FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) {
            throw new ClientFacingException("User not found", ErrorTypes.NotFound);
        }

        var newPasswordHash = newPassword.BuildSha256();

        user.PasswordHash = newPassword;
        user.ChangePasswordRequired = false;
        await _userRepository.UpdateAsync(user);
    }

    public async Task<int> CreateUserAsync(UserCreateModel user)
    {
        var userEntity = _mapper.Map<UserEntity>(user);

        return await _userRepository.CreateAsync(userEntity);
    }

    public async Task<int> CreateManyUsersAsync(List<UserCreateModel> users)
    {
        var userEntities = _mapper.Map<List<UserEntity>>(users);

        userEntities.ForEach(e => e.Status = UserStatus.Active);

        return await _userRepository.CreateRangeAsync(userEntities);
    }

    public async Task<int> UpdateUserAsync(UserUpdateModel user)
    {
        var userEntity = await _userRepository.Get()
            .Include(u => u.Role)
                .ThenInclude(e => e.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (userEntity == null) {
            throw new ClientFacingException("User not found", ErrorTypes.NotFound);
        }

        userEntity.FirstName = user.FirstName;
        userEntity.LastName = user.LastName;
        userEntity.UserName = user.UserName;
        userEntity.Email = user.Email;
        userEntity.DateOfBirth = user.DateOfBirth;
        userEntity.PhoneNumber = user.PhoneNumber;
        userEntity.Status = user.Status;

        return await _userRepository.UpdateAsync(userEntity);
    }

    public async Task<int> UpdateUserLoginDateAsync(long userId)
    {
        return await _userRepository.Get()
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(p =>
                p.SetProperty(u => u.LastLoginDate, DateTime.UtcNow));
    }

    public async Task<int> UpdateUserImageAsync(long userId, string? image)
    {
        return await _userRepository.Get()
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(p =>
                p.SetProperty(u => u.Image, image));
    }

    public async Task<bool> EmailExistAsync(long? userId, string email)
    {
        var emailExist = await _userRepository.Get()
            .Where(u => userId != null || u.Id != userId)
            .Where(u => u.Email.ToLower() == email.ToLower())
            .AnyAsync();

        return emailExist;
    }

    public async Task<List<UserModel>> GetUsersAsync()
    {
        var userEntities = await _userRepository.Get()
            .ToListAsync();

        var userDtos = _mapper.Map<List<UserModel>>(userEntities);

        return userDtos;
    }

    public Task<UserModel?> GetUserByIdAsync(long userId)
    {
        return InternalGetUserAsync(u => u.Id == userId);
    }

    public Task<UserModel?> GetUserByEmailAsync(string email)
    {
        return InternalGetUserAsync(u => u.Email == email);
    }

    private async Task<UserModel?> InternalGetUserAsync
        (Expression<Func<UserEntity, bool>> predicate)
    {
        var userEntity = await _userRepository.Get()
            .Include(u => u.Role)
                .ThenInclude(e => e.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(predicate);

        if (userEntity == null) {
            return null;
        }

        var userDto = _mapper.Map<UserModel>(userEntity);

        return userDto;
    }
}