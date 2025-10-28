FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Yandex.ApiGateway/Yandex.ApiGateway.csproj", "Yandex.ApiGateway/"]
RUN dotnet restore "Yandex.ApiGateway/Yandex.ApiGateway.csproj"
COPY . .
WORKDIR "/src/Yandex.ApiGateway"
RUN dotnet build "./Yandex.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Yandex.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Yandex.ApiGateway.dll"]
