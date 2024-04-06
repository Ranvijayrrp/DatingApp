using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public UsersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet] 
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
           try
            {
                var users = await _dataContext.Users.ToListAsync();
                return Ok(new {Data = users}) ;
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                new {Message = "Error while getting the users detail",
                Error = ex.Message
                });
            }
        }

        [HttpGet("id")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUserById(int id)
        {
           try
            {
                var user = await _dataContext.Users.FindAsync(id);
                return Ok(new {user}) ;
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                new {Message = "Error while getting the users detail",
                Error = ex.Message
                });
            }
        }
    }
}