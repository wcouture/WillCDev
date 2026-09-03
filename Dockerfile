# Step 1: Use the official ASP.NET Core 10.0 runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
EXPOSE 8080
EXPOSE 8081

# Step 2: Use the .NET 10 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

# Copy the project files and restore dependencies
WORKDIR /app
COPY ["WillCDev/WillCDev.csproj", "WillCDev/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "WillCDev/WillCDev.csproj"

# Copy the remaining source files and build the app
COPY . .
WORKDIR /app/WillCDev
RUN dotnet build -c $BUILD_CONFIGURATION -o ./build

# Step 3: Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# Note: Omitting --no-restore here is intentional for .NET 10 to ensure 
# static web assets in wwwroot (_framework) are packed correctly.
RUN dotnet publish -c $BUILD_CONFIGURATION -o ./publish

# Step 4: Build the final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/WillCDev/publish .
ENTRYPOINT ["dotnet", "WillCDev.dll"]
