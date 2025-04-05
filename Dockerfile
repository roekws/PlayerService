# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Copy project files
COPY ["src/PlayerService.API/PlayerService.API.csproj", "src/PlayerService.API/"]
COPY ["src/PlayerService.Core/PlayerService.Core.csproj", "src/PlayerService.Core/"]
RUN dotnet restore "src/PlayerService.API/PlayerService.API.csproj"

# Copy everything else
COPY ["src/", "src/"]

# Build
RUN dotnet build "src/PlayerService.API/PlayerService.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "src/PlayerService.API/PlayerService.API.csproj" -c Release -o /publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
COPY --from=publish /publish /app/

# # Copy dev certificate
# COPY ["src/PlayerService.API/https/cert.pfx", "app/https/cert.pfx"]

# Run
ENTRYPOINT ["dotnet", "/app/PlayerService.API.dll"]
