using Application.ExternalContracts.ResponseModels;
using Application.ExternalContracts.UserManagement;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VistaLOS.Application.Api.Attributes;
using VistaLOS.Application.Api.Controllers.Base;
using VistaLOS.Application.Common.Identity;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Services.Abstractions;
using VistaLOS.Application.Services.Models.UserManagement.User;

namespace VistaLOS.Application.Api.Controllers.UserManagement;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : TenantBaseController
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserContextAccessor userContextAccessor,
        IUserService userService,
        IMapper mapper)
        : base(userContextAccessor)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    [Permission(Permissions.ViewUser)]
    public async Task<ActionResult<DataResponse<List<UserDto>>>> GetUsers()
    {
        var users = await _userService.GetUsersAsync();
        return ApiResponse(users);
    }

    [HttpGet("{userId}")]
    [Permission(Permissions.ViewUser)]
    public async Task<ActionResult<DataResponse<UserDto?>>> GetUserById(long userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        return ApiResponse(user);
    }

    [HttpPost()]
    [Permission(Permissions.EditUser)]
    public async Task<ActionResult<BaseResponse>> AddUser(UserCreateIm user)
    {
        var userCreateDto = _mapper.Map<UserCreateDto>(user);
        await _userService.AddUserAsync(userCreateDto);
        return ApiResponse(new BaseResponse());
    }

    [HttpPut()]
    [Permission(Permissions.EditUser)]
    public async Task<ActionResult<BaseResponse>> UpdateUser(UserUpdateIm user)
    {
        var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
        await _userService.UpdateUserAsync(userUpdateDto);
        return ApiResponse(new BaseResponse());
    }

    [HttpPut("updateStatus")]
    [Permission(Permissions.EditUser)]
    public async Task<ActionResult<BaseResponse>> UpdateUserStatus(long userId, int status)
    {
        // ToDo: Think about implementing UserStatusEnum in several places.
        // await _userService.UpdateUserStatusAsync(user);
        return ApiResponse(new BaseResponse());
    }

    [HttpPut("updateImage")]
    [Permission(Permissions.EditUser)]
    public async Task<ActionResult<BaseResponse>> UpdateUserImage(long userId, string? image)
    {
        await _userService.UpdateUserImageAsync(userId, image);
        return ApiResponse(new BaseResponse());
    }
}