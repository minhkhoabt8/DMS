FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Gateways/OcelotGW/OcelotGW.csproj", "Gateways/OcelotGW/"]
RUN dotnet restore "Gateways/OcelotGW/OcelotGW.csproj"
COPY ["Gateways", "Gateways"]
WORKDIR "/src/Gateways/OcelotGW"
RUN dotnet build "OcelotGW.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OcelotGW.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OcelotGW.dll"]
