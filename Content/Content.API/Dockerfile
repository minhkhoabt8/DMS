FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Content/Content.API/Content.API.csproj", "Content/Content.API/"]
RUN dotnet restore "Content/Content.API/Content.API.csproj"
COPY ["Content", "Content"]
COPY ["AMQP", "AMQP"]
WORKDIR "/src/Content/Content.API"
RUN dotnet build "Content.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Content.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Content.API.dll"]
