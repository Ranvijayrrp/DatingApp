using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Implementations;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services,
        IConfiguration config)
        {
            //Added the databse configuration for EF core
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            //DI for class 
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}