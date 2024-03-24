using LogisticsCompany.Data;
using LogisticsCompany.Data.Factory;
using LogisticsCompany.Services;
using LogisticsCompany.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<LogisticsCompanyContext>();
builder.Services.AddSingleton<SqlDbFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(name: "default", "api/[controller]/{action}/{id?}");

app.Run();
