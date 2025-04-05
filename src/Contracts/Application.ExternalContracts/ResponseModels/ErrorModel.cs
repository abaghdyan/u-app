namespace Application.ExternalContracts.ResponseModels;

public class ErrorModel
{
    public string Message { get; set; }

    public ErrorModel(string message)
    {
        Message = message;
    }

    public static List<ErrorModel> WrapToErrorList(string message)
    {
        return new List<ErrorModel>() { new ErrorModel(message) };
    }
}
