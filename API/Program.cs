using System.Text;
using System.Text.Json.Serialization;
using API.Data;
using API.Extentions;
using API.Implementations;
using API.Interfaces;
using API.Middleware;
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

string MyAllowOrigins = string.Empty;

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowOrigins,
        builder =>
        {
            builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
        });
});

//custom services added here
builder.Services.AddApplicationService(configuration);

//added custom authentication
builder.Services.AddIdentityService(configuration);

var app = builder.Build();


//add migrations and seed data start //
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUser(context, logger);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured");
}

//add migrations and seed data end //


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMiddleware<ExceptionMiddleware>();
}

app.UseHttpsRedirection();

app.UseRouting();

//app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));
app.UseCors(MyAllowOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
