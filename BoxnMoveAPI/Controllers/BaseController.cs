using BoxnMove.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace BoxnMoveAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult Success<T>(T data, string message = null)
        {
            return Ok(new ServiceResponse<T>
            {
                Data = data,
                Status = 1,
                Message = message ?? "Operation successful."
            });
        }

        protected IActionResult Error(string message, int errorCode, bool isServerError = false)
        {
            return new ObjectResult(new ServiceResponse<bool>
            {
                Data = false,
                Status = 0,
                Message = message
            });
        }
    }
}
