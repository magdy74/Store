using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Magdy.APIs.Errors;
using Store.Magdy.Repository.Data.Contexts;

namespace Store.Magdy.APIs.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreDbContext _context;

        public BuggyController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("notfound")] // GET: /api/Buggy/notfound
        public async Task<IActionResult> GetNotFoundRequestError()
        {
            var brand = await _context.Brands.FindAsync(100);

            if (brand is null) return NotFound(new ApiErrorResponse(404, "Brand with Id: 100 is not found"));

            return Ok(brand);
        }

        [HttpGet("servererror")] // GET: /api/Buggy/servererror
        public async Task<IActionResult> GetServerError()
        {
            var brand = await _context.Brands.FindAsync(100);

            var brandToString = brand.ToString(); // will Throw Exception (NullReferenceException)
            
            return Ok(brand);

        }



        [HttpGet("badrequest")] // GET: /api/Buggy/badrequest
        public async Task<IActionResult> GetBadRequestError()
        {
            return BadRequest(new ApiErrorResponse(400));
        }


        [HttpGet("badrequest/{id}")] // GET: /api/Buggy/badrequest/ahmed
        public async Task<IActionResult> GetBadRequestError(int id) // Validation Error
        {
            return BadRequest();
        }

        [HttpGet("unauthorized")] // GET: /api/Buggy/unauthorized
        public async Task<IActionResult> GetUnauthorizedError(int id) // Validation Error
        {
            return Unauthorized(new ApiErrorResponse(401));
        }


    }
}
