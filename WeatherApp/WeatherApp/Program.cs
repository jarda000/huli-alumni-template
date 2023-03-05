using Microsoft.EntityFrameworkCore;
using WeatherApp.Contexts;
using WeatherApp.Interfaces;
using WeatherApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDbCityService, DbCityService>();
builder.Services.AddScoped<IDbUserService, DbUserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.MapControllers();

app.Run();

public partial class Program
{

}