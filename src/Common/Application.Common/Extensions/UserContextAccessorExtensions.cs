using VistaLOS.Application.Common.Exceptions;
using VistaLOS.Application.Common.Multitenancy;

namespace VistaLOS.Application.Common.Extensions;

public static class UserContextAccessorExtensions
{
    public static UserContext GetRequiredUserContext(this IUserContextAccessor accessor)
    {
        var userContext = accessor.GetUserContext();

        if (userContext == null) {
            throw new CoreException("UserContext was null");
        }

        return userContext;
    }
}
