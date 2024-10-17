using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using SimbirHealth.Account.Services.AccountService;
using SimbirHealth.Account.Services.AuthenticationService;
using SimbirHealth.Account.Services.DoctorService;
using SimbirHealth.Account.Services.TokenService;
using SimbirHealth.Common;
using SimbirHealth.Common.Services.Account;
using SimbirHealth.Common.Services.Db.Repositories;
using SimbirHealth.Common.Services.Web;
using SimbirHealth.Data.Models.Account;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var services = builder.Services;
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
// Add Swagger
ProgramService.ConfigureSwagger(services, "Account");
// Add DB
ProgramService.ConfigureNpgsql(services, builder.Configuration.GetConnectionString("DefaultConnection"));
// Add JWT Auth
ProgramService.ConfigureJwt(services, builder.Configuration.GetSection(JwtInfo.SectionName));
IdentityModelEventSource.ShowPII = true;


#region DI
services.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
services.AddTransient<IAuthenticationService, AuthenticationService>();
services.AddTransient<IAccountService, AccountService>();
services.AddTransient<IDoctorService, DoctorService>();
services.AddScoped<ITokenService, TokenService>();
#endregion


if (builder.Environment.IsProduction())
{
    builder.WebHost.UseKestrel().UseUrls("http://*:8080").UseIISIntegration();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
