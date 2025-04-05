using VistaLOS.Application.Services.Models.UserManagement.User;

namespace VistaLOS.Application.Services.Abstractions;

public interface IUserService
{
    Task AddUserAsync(UserCreateDto userCreateDto);
    Task<UserDto?> GetUserByIdAsync(long userId);
    Task<List<UserDto>> GetUsersAsync();
    Task UpdateUserAsync(UserUpdateDto userUpdateDto);
    Task UpdateUserImageAsync(long userId, string? image);
}
