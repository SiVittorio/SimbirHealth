using Microsoft.IdentityModel.Logging;
using SimbirHealth.Common.Services.Account;
using SimbirHealth.Common.Services.Web;
using SimbirHealth.Timetable.Models.Data;
using SimbirHealth.Timetable.Services.ExternalApiService;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddHttpClient();
// Add Swagger
ProgramService.ConfigureSwagger(services, "Timetable");
// Add DB
ProgramService.ConfigureNpgsql(services, builder.Configuration.GetConnectionString("DefaultConnection"));
// Add JWT Auth
ProgramService.ConfigureJwt(services, builder.Configuration.GetSection(JwtInfo.SectionName));
IdentityModelEventSource.ShowPII = true;

services.AddOptions();
services.Configure<ExternalApiRoutes>(builder.Configuration.GetSection(nameof(ExternalApiRoutes)));

#region DI
services.AddScoped<IExternalApiService, ExternalApiService>();
#endregion

if (builder.Environment.IsProduction())
{
    builder.WebHost.UseKestrel().UseUrls("http://*:8082").UseIISIntegration();
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

