using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly DataContext _data;

        public HomeController(DataContext data)
        {
            _data = data;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await _data.Users.ToListAsync();
                return Ok(new { users });
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                new
                {
                    Message = "Error while getting the users detail",
                    Error = ex.Message
                });
            }
        }
    }
}