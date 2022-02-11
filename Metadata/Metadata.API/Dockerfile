FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Metadata/Metadata.API/Metadata.API.csproj", "Metadata/Metadata.API/"]
RUN dotnet restore "Metadata/Metadata.API/Metadata.API.csproj"
COPY ["Metadata", "Metadata"]
COPY ["AMQP", "AMQP"]
WORKDIR "/src/Metadata/Metadata.API"
RUN dotnet build "Metadata.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Metadata.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Metadata.API.dll"]
