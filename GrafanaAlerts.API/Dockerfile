# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["GrafanaAlerts.API/GrafanaAlerts.API.csproj", "GrafanaAlerts.API/"]
COPY ["GrafanaAlerts.Core/GrafanaAlerts.Core.csproj", "GrafanaAlerts/Core"]
COPY ["GrafanaAlerts.Infrastructure/GrafanaAlerts.Infrastructure.csproj", "GrafanaAlerts.Infrustructure"]

RUN dotnet restore "GrafanaAlerts.API/GrafanaAlerts.API.csproj"

# Copy everything else and build
COPY . .

RUN dotnet build "GrafanaAlerts.API/GrafanaAlerts.API.csproj" -c Release -o /app/build


RUN dotnet publish "GrafanaAlerts.API/GrafanaAlerts.API.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0.0
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "GrafanaAlerts.API.dll"]