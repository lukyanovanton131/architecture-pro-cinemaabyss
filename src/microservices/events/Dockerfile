FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

EXPOSE 8082
ENV ASPNETCORE_HTTP_PORTS=8082

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Yandex.Cinema.Events/Yandex.Cinema.Events.csproj", "Yandex.Cinema.Events/"]
RUN dotnet restore "Yandex.Cinema.Events/Yandex.Cinema.Events.csproj"
COPY . .
WORKDIR "/src/Yandex.Cinema.Events"
RUN dotnet build "Yandex.Cinema.Events.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Yandex.Cinema.Events.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Yandex.Cinema.Events.dll"]
