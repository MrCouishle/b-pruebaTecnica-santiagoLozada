using Microsoft.AspNetCore.Mvc;

namespace Services
{
    public class BaseService : ControllerBase
    {
        protected ActionResult SendResponse<T>(Response<T> response)
        {
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
