FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["GrafanaAlerts.MnemonicForm.csproj", "."]
RUN dotnet restore "GrafanaAlerts.MnemonicForm.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "GrafanaAlerts.MnemonicForm.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrafanaAlerts.MnemonicForm.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
RUN apt-get update && apt-get install telnet
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrafanaAlerts.MnemonicForm.dll"]
