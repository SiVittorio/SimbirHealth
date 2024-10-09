using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SimbirHealth.Common.Services.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimbirHealth.Common.Services.Web
{
    /// <summary>
    /// Класс для настройки WebAPI проектов
    /// </summary>
    public static class ProgramService
    {
        /// <summary>
        /// Настройка Swagger с авторизацией Bearer 
        /// </summary>
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Используется JWT Bearer-авторизация. \n\n Для использования токена, введите слово \"Bearer\" и затем добавьте токен, например так: Bearer 1231sdksk",
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
            });
            return services;
        }

        /// <summary>
        /// Настройка контекста БД для PostgreSQL
        /// </summary>
        public static IServiceCollection ConfigureNpgsql(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<SimbirHealthContext>(options =>
            options.UseNpgsql(
                connectionString
                )
            );
            return services;
        }

        public static IServiceCollection ConfigureJwt(this  IServiceCollection services, IConfigurationSection? jwtSection)
        {
            services.Configure<JwtInfo>(jwtSection);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                options => options.TokenValidationParameters = AccountTokenValidationParameters.DefaultParameters(jwtSection.Get<JwtInfo>()!));
            // TODO Возможно надо будет добавить кастомные политики -> services.AddAuthorizationBuilder().AddPolicy()
            return services;
        }
    }
}
