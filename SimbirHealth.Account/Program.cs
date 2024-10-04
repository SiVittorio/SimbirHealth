using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using SimbirHealth.Account.Models.Info;
using SimbirHealth.Account.Services.AccountService;
using SimbirHealth.Account.Services.AuthenticationService;
using SimbirHealth.Account.Services.DoctorService;
using SimbirHealth.Account.Services.TokenService;
using SimbirHealth.Common;
using SimbirHealth.Common.Repositories;
using SimbirHealth.Data.Models.Account;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var services = builder.Services;
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(swagger =>
{
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
              new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });

    var basePath = AppContext.BaseDirectory;

    var xmlPath = Path.Combine(basePath, "SimbirHealth.Account.xml");
    swagger.IncludeXmlComments(xmlPath);
}
);

// Configure db context
services.AddDbContext<SimbirHealthContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
        )
    );
#region DI
services.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
services.AddTransient<IAuthenticationService, AuthenticationService>();
services.AddTransient<IAccountService, AccountService>();
services.AddTransient<IDoctorService, DoctorService>();
services.AddScoped<ITokenService, TokenService>();
#endregion

var jwtSection = builder.Configuration.GetSection(JwtInfo.SectionName);
services.Configure<JwtInfo>(jwtSection);

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
    options => options.TokenValidationParameters = AccountTokenValidationParameters.DefaultParameters(jwtSection.Get<JwtInfo>()!));

// TODO ¬озможно надо будет добавить кастомные политики -> services.AddAuthorizationBuilder().AddPolicy()


IdentityModelEventSource.ShowPII = true;

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
