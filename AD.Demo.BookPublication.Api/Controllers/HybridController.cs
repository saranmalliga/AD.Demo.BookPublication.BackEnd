using AD.Demo.BookPublication.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AD.Demo.BookPublication.Api.Controllers
{

    public class HybridController : ControllerBase
    {
        //Common controller functionality implement here
        protected readonly ILogger<HybridController> Logger;
        private string _exceptionMessage = "Exception occured while processing request.";
        public HybridController(ILogger<HybridController> logger, IConfiguration configuration)
        {
            Logger = logger;
        }
        protected IActionResult HandleUserException(Exception ex)
        {
            Logger.LogError(ex, $"UserException: {_exceptionMessage}");
            Logger.LogError(ex, _exceptionMessage);
            return BadRequest(ServiceResponse.ErrorResponse(ex));
        }

        protected IActionResult HandleOtherException(Exception ex)
        {
            Logger.LogError(ex, $"OtherException: {_exceptionMessage}");
            var response = ServiceResponse.ErrorResponse(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }
}
