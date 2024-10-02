# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["SimbirHealth.Account/SimbirHealth.Account.csproj", "SimbirHealth.Account/"]
COPY ["SimbirHealth.Common/SimbirHealth.Common.csproj", "SimbirHealth.Common/"]
COPY ["SimbirHealth.Data/SimbirHealth.Data.csproj", "SimbirHealth.Data/"]
RUN dotnet restore "./SimbirHealth.Account/SimbirHealth.Account.csproj"
COPY . .
WORKDIR "/src/SimbirHealth.Account"
RUN dotnet build "./SimbirHealth.Account.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./SimbirHealth.Account.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimbirHealth.Account.dll"]