using Microsoft.AspNetCore.Http;

namespace Application.ExternalContracts.ResponseModels;

public class DataResponse<T> : BaseResponse
{
    public T? Data { get; set; }

    public DataResponse()
    {

    }

    public DataResponse(T data)
    {
        Data = data;
    }


    public static DataResponse<T> BuildFailedResponse(BaseResponse failedResponse) =>
        BuildFailedResponseInternal<DataResponse<T>>(failedResponse);

    public static DataResponse<T> BuildFailedResponse(string message, int? statusCode = null) =>
        BuildFailedResponseInternal<DataResponse<T>>(message, statusCode);

    protected static TResponse BuildFailedResponseInternal<TResponse>(BaseResponse failedResponse)
        where TResponse : DataResponse<T>, new()
    {
        return new TResponse {
            Data = default,
            Errors = failedResponse.Errors,
            StatusCode = failedResponse.StatusCode,
            GeneratedAt = failedResponse.GeneratedAt
        };
    }

    protected static TResponse BuildFailedResponseInternal<TResponse>(string message, int? statusCode = null)
        where TResponse : DataResponse<T>, new()
    {
        return new TResponse {
            Data = default,
            Errors = ErrorModel.WrapToErrorList(message),
            StatusCode = statusCode ?? StatusCodes.Status400BadRequest,
            GeneratedAt = DateTime.UtcNow
        };
    }
}
