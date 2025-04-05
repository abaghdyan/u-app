using VistaLOS.Application.Data.Common.Options;

namespace VistaLOS.Application.Data.Master.Options;

public class MasterDbOptions : DbBaseOptions
{
    public const string Section = "MasterDb";

    public string EncryptionKey { get; set; } = null!;
}
