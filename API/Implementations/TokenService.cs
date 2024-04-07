using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace API.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.
            GetBytes(_configuration.GetSection("TokenKey").Value));
        }
        public string CreateToken(AppUser user)
        {
            try
            {
                 var  claims = new List<Claim>{
                    new Claim (Microsoft.IdentityModel.JsonWebTokens.
                    JwtRegisteredClaimNames.NameId, user.UserName),
                 };

                 var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

                 var tokenDescriptor = new SecurityTokenDescriptor{
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                 };

                 var tokenHandler = new JwtSecurityTokenHandler();

                 var token = tokenHandler.CreateToken(tokenDescriptor);

                 return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}