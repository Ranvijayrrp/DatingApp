using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext dataContext, ITokenService tokenService)
        {
            _dataContext = dataContext;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        {
            try
            {

                var userDetail = await _dataContext.Users.SingleOrDefaultAsync(
                    user => user.UserName.Equals(loginDto.Username)
                    );

                if (userDetail is null) return Unauthorized("Please enter valid username.");

                var hmac = new HMACSHA512(userDetail.PasswordSalt);

                var hashPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

                for (int i = 0; i < hashPassword.Length - 1; i++)
                {
                    if (hashPassword[i] != userDetail.PasswordHash[i])
                        return Unauthorized("Please enter valid password");
                }

                if (!hashPassword.SequenceEqual(userDetail.PasswordHash))
                {
                    return Unauthorized("Please enter valid password");
                }

                UserDto userDto = CreateTokenAndReturn(userDetail);

                //return Ok(new { Message = $"Conguratulations {userDto.Username} !! you are login", Data = userDto });
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                new { Message = "Error while login the user.", Error = ex.Message });
            }
        }

        private UserDto CreateTokenAndReturn(AppUser userDetail)
        {
            return new UserDto
            {
                Username = userDetail.UserName,
                Token = _tokenService.CreateToken(userDetail),
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            try
            {
                if (registerDto is null
                ||
                (
                    registerDto is not null
                    && (string.IsNullOrWhiteSpace(registerDto.Username)
                    || string.IsNullOrWhiteSpace(registerDto.Password))

                )) return BadRequest(new { Message = $"Please enter correct username and password" });


                if (await UserExists(registerDto.Username))
                    return BadRequest(new { Message = "User already exists." });

                using var hmac = new HMACSHA512();
                var user = new AppUser()
                {
                    UserName = registerDto.Username,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                    PasswordSalt = hmac.Key
                };

                _dataContext.Users.Add(user);
                await _dataContext.SaveChangesAsync();

                var userDto = CreateTokenAndReturn(user);

                return Ok(userDto);
                //return Ok(new { UserDetails = userDto, Message = $"User : {userDto.Username} registered successfully," });
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