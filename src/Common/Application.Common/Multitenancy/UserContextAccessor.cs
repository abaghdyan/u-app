namespace VistaLOS.Application.Common.Multitenancy;

internal class UserContextAccessor : IUserContextAccessor
{
    private readonly UserContextHolder _userContextHolder;

    public UserContextAccessor(UserContextHolder userContextHolder)
    {
        _userContextHolder = userContextHolder;
    }

    public UserContext? GetUserContext()
    {
        return _userContextHolder.UserContext;
    }
}
