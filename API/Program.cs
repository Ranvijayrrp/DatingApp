using System.Text;
using API.Data;
using API.Implementations;
using API.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//configuration to run using HTTPS start

var configuration = builder.Configuration //new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("./Properties/launchSettings.json")
    .Build();

var appUrl = configuration["profiles:https:applicationUrl"];
builder.WebHost.UseUrls(appUrl);

//configuration to run using HTTPS end

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddCors();

string  MyAllowOrigins = string.Empty;

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowOrigins,
        builder =>
        {
            builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
        });
});

//adding authorization

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(Options =>
   Options.TokenValidationParameters = new TokenValidationParameters
   {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.
      GetBytes(configuration.GetSection("TokenKey").Value)),
      ValidateIssuer = false,
      ValidateAudience = false
   }
);

//Added the databse configuration for EF core
builder.Services.AddDbContext<DataContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//DI for class 
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

//app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));
app.UseCors(MyAllowOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
