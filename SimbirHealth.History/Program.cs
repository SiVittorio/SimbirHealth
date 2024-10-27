using Microsoft.IdentityModel.Logging;
using SimbirHealth.Common.Services.Account;
using SimbirHealth.Common.Services.Db.Repositories;
using SimbirHealth.Common.Services.Web;
using SimbirHealth.Common.Services.Web.AuthValidationService;
using SimbirHealth.Common.Services.Web.ExternalApiService;
using SimbirHealth.History.Services.HistoryService;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddHttpClient();

// Add Swagger
ProgramService.ConfigureSwagger(services, "History");
// Add DB
ProgramService.ConfigureNpgsql(services, builder.Configuration.GetConnectionString("DefaultConnection"));
// Add JWT Auth
ProgramService.ConfigureJwt(services, builder.Configuration.GetSection(JwtInfo.SectionName));
IdentityModelEventSource.ShowPII = true;

services.AddOptions();
services.Configure<ExternalApiRoutes>(builder.Configuration.GetSection(nameof(ExternalApiRoutes)));

#region DI
services.AddScoped<IExternalApiService, ExternalApiService>();
services.AddScoped<IHistoryService, HistoryService>();
services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
services.AddScoped<IAuthValidationService, AuthValidationService>();
#endregion

if (builder.Environment.IsProduction())
{
    builder.WebHost.UseKestrel().UseUrls("http://*:8083").UseIISIntegration();
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();