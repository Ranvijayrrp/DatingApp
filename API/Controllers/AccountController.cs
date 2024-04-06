using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public AccountController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(RegisterDto register)
        {
            try
            {
                var hmac = new HMACSHA512();
                var userDetail = await _dataContext.Users.SingleOrDefaultAsync(
                    user => user.UserName.Equals(register.Username)
                    );

                if (userDetail is null) return BadRequest("Please enter valid username.");

                var hashPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password));

                if (userDetail.UserName.Equals(register.Username) && hashPassword.Equals(register.Username))
                    return Ok(new { Message = "Congratulations !! you are able to login.", Data = userDetail });

                return Ok(new AppUser());
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                new { Message = "Error while login the user.", Error = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            try
            {
                if (registerDto is null
                ||
                (
                    registerDto is not null
                    && (registerDto.Username is null
                    || registerDto.Password is null)

                )) return BadRequest(new { Message = $"Please enter correct username and password" });


                if (await UserExists(registerDto.Username))
                    return BadRequest("User already exists.");

                using var hmac = new HMACSHA512();
                var user = new AppUser()
                {
                    UserName = registerDto.Username,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                    PasswordSalt = hmac.Key
                };

                _dataContext.Users.Add(user);
                await _dataContext.SaveChangesAsync();
                return Ok(new { UserDetails = user, Message = $"User : {registerDto.Username} registered successfully," });
            }
            catch (Exception ex)
            {
                return StatusCode(500,
               new
               {
                   Message = "Error while registering the user detail",
                   Error = ex.Message
               });
            }
        }

        private async Task<bool> UserExists(string UserName)
        {
            return await _dataContext.Users.AnyAsync(user => user.UserName.Equals(UserName));
        }
    }
}