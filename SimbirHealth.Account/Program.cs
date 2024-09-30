using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using SimbirHealth.Account.Models.Info;
using SimbirHealth.Account.Services.AuthenticationService;
using SimbirHealth.Account.Services.TokenService;
using SimbirHealth.Common;
using SimbirHealth.Common.Repositories;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
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
}
);

// Configure db context
builder.Services.AddDbContext<SimbirHealthContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("MyPrivateConnection")
        )
    );

IdentityModelEventSource.ShowPII = true;

#region DI
builder.Services.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService,  TokenService>();
#endregion

builder.Services.Configure<JwtInfo>(builder.Configuration.GetSection(JwtInfo.SectionName));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
