using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using Application.ExternalContracts.ResponseModels;
using Newtonsoft.Json;
using VistaLOS.Application.Api.Options;
using VistaLOS.Application.Common.Constants;
using VistaLOS.Application.Common.Exceptions;
using VistaLOS.Application.Common.Helpers;

namespace VistaLOS.Application.Api.Middlewares;

public class GlobalExceptionHandler : IMiddleware
{
    private readonly ILogger _logger;
    private readonly LoggingConfigOptions _loggingOptions;

    public GlobalExceptionHandler(ILogger logger, LoggingConfigOptions loggingOptions)
    {
        _loggingOptions = loggingOptions;
        _logger = logger.ForContext<GlobalExceptionHandler>();
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try {
            var traceId = Activity.Current?.TraceId.ToString();
            if (traceId != null) {
                context.Response.Headers.TryAdd("X-TraceId", traceId);
            }
            await next(context);
        }
        catch (Exception ex) {
            LogException(_logger, ex);
            await WriteExceptionToResponse(ex, context);
        }
    }

    private static void LogException(ILogger logger, Exception ex)
    {
        if (ex is ApplicationException appException) {
            logger.Error(appException.Message, ex);
        }
        else {
            logger.Error("Unhandled exception", ex, ex.Source ?? string.Empty);
        }
    }

    private async Task WriteExceptionToResponse(Exception ex, HttpContext httpContext)
    {
        var statusCode = GetStatusCode(ex);
        var errorMessages = GetErrorMessages(ex);

        HttpResponse response = httpContext.Response;
        response.ContentType = MediaTypeNames.Application.Json;
        response.StatusCode = statusCode;
        var baseResponse = new BaseResponse(statusCode, errorMessages.ToArray());
        await httpContext.Response.WriteAsync(JsonHelper.SerializeObject(baseResponse));
    }

    private IEnumerable<string> GetErrorMessages(Exception ex)
    {
        switch (ex) {
            case ClientFacingException clientFacingException:
                yield return clientFacingException.Message;
                break;
            default:
                yield return "Something went wrong";
                if (_loggingOptions.EnableDetailedLogging) {
                    yield return ex.Message;
                    if (ex.StackTrace != null)
                        yield return ex.StackTrace;
                }
                break;
        }
    }

    private static int GetStatusCode(Exception ex)
    {
        return ex switch {
            CoreException coreException => coreException.Code ?? (int)HttpStatusCode.BadRequest,
            ValidationException => (int)HttpStatusCode.BadRequest,
            TaskCanceledException or OperationCanceledException => StatusCodes.Status408RequestTimeout,
            _ => (int)HttpStatusCode.InternalServerError
        };
    }
}
