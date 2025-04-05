namespace VistaLOS.Application.Common.Multitenancy;

public interface IUserContextAccessor
{
    UserContext? GetUserContext();
}
