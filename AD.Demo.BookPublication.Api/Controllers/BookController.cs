using AD.Demo.BookPublication.Api.CustomActionFilter;
using AD.Demo.BookPublication.Domain;
using AD.Demo.BookPublication.Domain.DTO;
using AD.Demo.BookPublication.Interfaces.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace AD.Demo.BookPublication.Api.Controllers
{
    [ApiController]
    [Route("api/book")]
    //[Authorize]
    [HasRoleAccess]
    public class BookController : HybridController
    {
        private readonly IBookService _bookService;

        public BookController(ILogger<BookController> logger, IConfiguration config, IBookService bookService) : base(logger, config)  
        {
            _bookService = bookService;
        }


        [HttpGet, Route("GetAllBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBooks(string? filterBy, string? filterValue, string? sortBy, string? sortDir, int? pageIndex, int? pageSize)
        {
            try
            {
                var result = await _bookService.GetAllBooks(filterBy, filterValue, sortBy, sortDir, pageIndex, pageSize);
                return Ok(ServiceResponse.SuccessResponse(Constants.ServiceResponseMessages.SuccessMessage, result));
            }
            catch (ArgumentException ex)
            {
                return HandleUserException(ex);
            }
            catch (Exception ex)
            {
                return HandleOtherException(ex);
            }
        }

        [HttpPost, Route("ImportSampleBookData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ImportSampleBookData(IList<BookDTO> books)
        {
            try
            {
                var result = await _bookService.ImportSampleBookData(books);
                return  Ok(ServiceResponse.SuccessResponse(result.Success ? Constants.ServiceResponseMessages.SuccessMessage : Constants.ServiceResponseMessages.ErrorMessage, result));
            }
            catch (ArgumentException ex)
            {
                return HandleUserException(ex);
            }
            catch (Exception ex)
            {
                return HandleOtherException(ex);
            }
        }
    }
}
