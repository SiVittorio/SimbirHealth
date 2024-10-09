using Microsoft.IdentityModel.Logging;
using SimbirHealth.Common.Services.Account;
using SimbirHealth.Common.Services.Db.Repositories;
using SimbirHealth.Common.Services.Web;
using SimbirHealth.Hospital.Services.HospitalService;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var services = builder.Services;
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
// Настройка Swagger
ProgramService.ConfigureSwagger(services, "Hospital");
// Настройка БД
ProgramService.ConfigureNpgsql(services, builder.Configuration.GetConnectionString("DefaultConnection"));
ProgramService.ConfigureJwt(services, builder.Configuration.GetSection(JwtInfo.SectionName));
IdentityModelEventSource.ShowPII = true;

#region DI
services.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
services.AddTransient<IHospitalService, HospitalService>();
#endregion


if (builder.Environment.IsProduction())
{
    builder.WebHost.UseKestrel().UseUrls("http://*:8081").UseIISIntegration();
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
