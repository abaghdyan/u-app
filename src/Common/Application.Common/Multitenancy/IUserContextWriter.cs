namespace VistaLOS.Application.Common.Multitenancy;

public interface IUserContextWriter
{
    void SetUserContext(UserContext userContext);
}
