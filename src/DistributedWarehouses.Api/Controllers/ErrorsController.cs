using DistributedWarehouses.Domain.Exceptions;
using DistributedWarehouses.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DistributedWarehouses.Api.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        [Route("error")]
        public ErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;
            var code = 500;

            if (exception is BaseException customException)
            {
                code = (int)customException.StatusCode;
            }
            Response.StatusCode = code;
            return new ErrorResponse{Message = exception.Message};
        }
    }
}
