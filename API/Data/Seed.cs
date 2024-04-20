using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUser(DataContext dataContext,ILogger logger)
        {
            try
            {
                if(await dataContext.Users.AnyAsync()) return;
    
                var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
                var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
                
                // new JsonSerializerOptions{
                //     ReferenceHandler = ReferenceHandler.Preserve,
                //     MaxDepth = int.MaxValue
                // });
    
                foreach(var user in users)
                {
                    using var hmac = new HMACSHA512();
                    user.UserName = user.UserName.ToLower();
                    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd")); 
                    user.PasswordSalt = hmac.Key;
    
                    dataContext.Users.Add(user);  
                }
    
                await dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
               logger.LogError(ex,"Error while processing json data of user");
            }
        }
    }
}