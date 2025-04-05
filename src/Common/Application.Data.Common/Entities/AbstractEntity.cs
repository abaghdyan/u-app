namespace VistaLOS.Application.Data.Common.Entities;

public abstract class AbstractEntity
{
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public DateTime? DeletedDate { get; set; }
}
