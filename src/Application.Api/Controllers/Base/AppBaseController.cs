using Application.ExternalContracts.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace VistaLOS.Application.Api.Controllers.Base;

public abstract class AppBaseController : ControllerBase
{
    protected ActionResult<DataResponse<T>> ApiResponse<T>(T data)
    {
        return Ok(new DataResponse<T>(data));
    }

    protected ActionResult<DataResponse<T>> ApiResponse<T>(DataResponse<T> dataResponse)
    {
        return StatusCode(dataResponse.StatusCode, dataResponse);
    }

    protected ActionResult<BaseResponse> ApiResponse(BaseResponse baseResponse)
    {
        return StatusCode(baseResponse.StatusCode, baseResponse);
    }
}
