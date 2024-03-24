using AutoMapper;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Factory;
using LogisticsCompany.Mapping.Profiles;
using LogisticsCompany.Services;
using LogisticsCompany.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add AutoMapper
builder.Services.AddAutoMapper(mapper =>
{
    mapper.AddProfile(new MappingProfile());
});


// Register Context and Database Factory
builder.Services.AddSingleton<LogisticsCompanyContext>();
builder.Services.AddSingleton<SqlDbFactory>();

// Register Database Services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRoleService, RoleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(name: "default", "api/[controller]/{action}/{id?}");

app.Run();
