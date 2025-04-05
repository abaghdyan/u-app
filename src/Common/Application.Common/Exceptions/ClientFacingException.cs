using VistaLOS.Application.Common.Enums;

namespace VistaLOS.Application.Common.Exceptions;

public class ClientFacingException : Exception
{
    public ErrorTypes? ErrorType { get; set; }


    public ClientFacingException(string? message, ErrorTypes? errorType = null) : base(message)
    {
        ErrorType = errorType;
    }
}
