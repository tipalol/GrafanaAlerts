FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["GrafanaAlerts.API/GrafanaAlerts.API.csproj", "GrafanaAlerts.API/"]
COPY ["GrafanaAlerts.Core/GrafanaAlerts.Core.csproj", "GrafanaAlerts.Core//"]
COPY ["GrafanaAlerts.Infrastructure/GrafanaAlerts.Infrastructure.csproj", "GrafanaAlerts.Infrastructure/"]

RUN dotnet restore "GrafanaAlerts.API/GrafanaAlerts.API.csproj"
COPY . .
WORKDIR "/src/GrafanaAlerts.API"
RUN dotnet build "GrafanaAlerts.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrafanaAlerts.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "GrafanaAlerts.API.dll"]