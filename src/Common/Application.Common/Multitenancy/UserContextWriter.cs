namespace VistaLOS.Application.Common.Multitenancy;

internal class UserContextWriter : IUserContextWriter
{
    private readonly UserContextHolder _userContextHolder;

    public UserContextWriter(UserContextHolder userContextHolder)
    {
        _userContextHolder = userContextHolder;
    }

    public void SetUserContext(UserContext userContext)
    {
        _userContextHolder.UserContext = userContext;
    }
}
