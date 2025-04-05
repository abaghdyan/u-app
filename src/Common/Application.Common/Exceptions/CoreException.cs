using System.Net;

namespace VistaLOS.Application.Common.Exceptions;

public class CoreException : Exception
{
    public int? Code { get; set; }

    public CoreException(string? message, int? code = null) : base(message)
    {
        Code = code;
    }

    public CoreException(string? message, Exception innerException)
        : base(message, innerException)
    {
    }

    public CoreException(string message, HttpStatusCode httpStatusCode)
        : this(message, (int)httpStatusCode)
    {
    }
}
