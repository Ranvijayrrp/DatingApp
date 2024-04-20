using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository repository,IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
          
        }

        [HttpGet] 
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
           try
            {
                var users = await _repository.GetMembersAsync();
                //var userDtos = _mapper.Map<IEnumerable<MemberDto>>(users); 
                return Ok(new { Data = users });
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                new {Message = "Error while getting the users detail",
                Error = ex.Message
                });
            }
        }

        
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUserByUsername([FromRoute] string username)
        {
           try
            {
                var user = await _repository.GetMemberAsync(username);
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

        
        // [HttpGet("api/users/{id}")]
        // public async Task<ActionResult<IEnumerable<AppUser>>> GetUserById([FromRoute] int id)
        // {
        //    try
        //     {
        //         var user = await _repository.GetUserByIdAsync(id);
        //         return Ok(new {user}) ;
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500,
        //         new {Message = "Error while getting the users detail",
        //         Error = ex.Message
        //         });
        //     }
        // }
    }
}