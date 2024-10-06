using System.Text;
using LogisticsCompany.Data;
using LogisticsCompany.Data.Common.Adapters;
using LogisticsCompany.Data.Contracts;
using LogisticsCompany.Data.Factory;
using LogisticsCompany.Mapping.Profiles;
using LogisticsCompany.Services.Authorization;
using LogisticsCompany.Services.Deliveries.Commands;
using LogisticsCompany.Services.Deliveries.Queries;
using LogisticsCompany.Services.Offices.Commands;
using LogisticsCompany.Services.Offices.Queries;
using LogisticsCompany.Services.Package.Commands;
using LogisticsCompany.Services.Package.Queries;
using LogisticsCompany.Services.PackageStatuses.Queries;
using LogisticsCompany.Services.Reports.Queries;
using LogisticsCompany.Services.Roles.Queries;
using LogisticsCompany.Services.Users.Commands;
using LogisticsCompany.Services.Users.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add AutoMapper
builder.Services.AddAutoMapper(mapper =>
{
    mapper.AddProfile(new MappingProfile());
});


var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnectionString");

// Register Context and Database Factory
builder.Services.AddSingleton<LogisticsCompanyContext>();
builder.Services.AddSingleton<SqlDbFactory>();
builder.Services.AddSingleton<IDbAdapter, SqlDbAdapter>(_ => new SqlDbAdapter(connectionString));

// Register Authorization Services

builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();

// Register Query Services
builder.Services.AddTransient<IUserQueryService, UserQueryService>();
builder.Services.AddTransient<IRoleQueryService, RoleQueryService>();
builder.Services.AddTransient<IOfficeQueryService, OfficeQueryService>();

builder.Services.AddTransient<IPackageQueryService, PackageQueryService>();
builder.Services.AddTransient<IPackageStatusQueryService, PackageStatusQueryService>();

builder.Services.AddTransient<IDeliveryQueryService, DeliveryQueryService>();
builder.Services.AddTransient<IReportQueryService, ReportQueryService>();


// Register Command Services
builder.Services.AddTransient<IUserCommandService, UserCommandService>();
builder.Services.AddTransient<IOfficeCommandService, OfficeCommandService>();
builder.Services.AddTransient<IPackageCommandService, PackageCommandService>();
builder.Services.AddTransient<IDeliveryCommandService, DeliveryCommandService>();

// Register JWT Services
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         ValidAudience = jwtIssuer,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
     };
 });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();

});

using (var scope = app.Services.CreateScope())
{
    var dbFactory = scope.ServiceProvider.GetRequiredService<LogisticsCompanyContext>();
    dbFactory.InitializeDatabase();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(name: "default", "api/[controller]/{action}/{id?}");

app.Run();
