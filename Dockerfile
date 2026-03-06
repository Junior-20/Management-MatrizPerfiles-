FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project file and restore as distinct layers
COPY ["MatrizPerfiles.Web/MatrizPerfiles.Web.csproj", "MatrizPerfiles.Web/"]
RUN dotnet restore "./MatrizPerfiles.Web/MatrizPerfiles.Web.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/MatrizPerfiles.Web"
RUN dotnet build "./MatrizPerfiles.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MatrizPerfiles.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MatrizPerfiles.Web.dll"]
