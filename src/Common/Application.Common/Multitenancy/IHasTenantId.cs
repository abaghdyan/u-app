namespace VistaLOS.Application.Common.Multitenancy;

public interface IHasTenantId
{
    string TenantId { get; set; }
}
