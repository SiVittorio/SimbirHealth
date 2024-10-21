FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8082

ENV ASPNETCORE_URLS=http://+:8082

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["SimbirHealth.Timetable/SimbirHealth.Timetable.csproj", "SimbirHealth.Timetable/"]
RUN dotnet restore "SimbirHealth.Timetable/SimbirHealth.Timetable.csproj"
COPY . .
WORKDIR "/src/SimbirHealth.Timetable"
RUN dotnet build "SimbirHealth.Timetable.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "SimbirHealth.Timetable.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimbirHealth.Timetable.dll"]
