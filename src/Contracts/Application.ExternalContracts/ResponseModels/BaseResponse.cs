using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using VistaLOS.Application.Common.Interfaces;

namespace Application.ExternalContracts.ResponseModels;

public class BaseResponse : IHasStatusCode, IHasSuccess
{
    public bool Success => IsSuccessStatusCode(StatusCode);
    public int StatusCode { get; set; }
    public string? TraceId { get; }
    public DateTime GeneratedAt { get; protected init; }
    public IReadOnlyList<ErrorModel> Errors { get; protected set; }

    public BaseResponse()
    {
        GeneratedAt = DateTime.UtcNow;
        StatusCode = StatusCodes.Status200OK;
        Errors = new List<ErrorModel>();
        TraceId = Activity.Current?.TraceId.ToString();
    }
    public BaseResponse(int statusCode)
    : this()
    {
        StatusCode = statusCode;
    }

    public BaseResponse(int statusCode, params string[] errors) :
        this(statusCode, errors.Select(w => new ErrorModel(w)))
    {
    }

    public BaseResponse(int statusCode, IEnumerable<ErrorModel> errorModels) : this()
    {
        StatusCode = statusCode;
        Errors = errorModels.ToArray();
    }

    public void AddErrors(IEnumerable<ErrorModel>? errorModels)
    {
        var errorsArray = errorModels?.ToArray() ?? Array.Empty<ErrorModel>();

        if (errorsArray.Length > 0) {
            Errors = Errors.Concat(errorsArray).ToList();
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    public void AddErrors(params ErrorModel[] errorModel)
    {
        AddErrors(errorModel.AsEnumerable());
    }

    private static bool IsSuccessStatusCode(int statusCode)
    {
        return statusCode >= 200 && statusCode <= 299;
    }
}