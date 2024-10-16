FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8081

ENV ASPNETCORE_URLS=http://+:8081

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["SimbirHealth.Hospital/SimbirHealth.Hospital.csproj", "SimbirHealth.Hospital/"]
RUN dotnet restore "SimbirHealth.Hospital/SimbirHealth.Hospital.csproj"
COPY . .
WORKDIR "/src/SimbirHealth.Hospital"
RUN dotnet build "SimbirHealth.Hospital.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "SimbirHealth.Hospital.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimbirHealth.Hospital.dll"]
