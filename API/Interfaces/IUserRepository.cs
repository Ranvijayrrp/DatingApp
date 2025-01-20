using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        /// <summary>
        /// Asynchronously retrieves a member's details based on their username.
        /// </summary>
        /// <param name="username">The username of the member to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the member details as a <see cref="MemberDto"/>.</returns>
        Task<MemberDto> GetMemberAsync(string username);
        Task<IEnumerable<MemberDto>> GetMembersAsync();
    }
}