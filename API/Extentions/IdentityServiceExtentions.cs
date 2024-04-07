using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Extentions
{
    public static class IdentityServiceExtentions
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services,
        IConfiguration config)
        {

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(Options =>
               Options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.
                  GetBytes(config.GetSection("TokenKey").Value)),
                   ValidateIssuer = false,
                   ValidateAudience = false
               }
            );

            return services;
        }
    }
}