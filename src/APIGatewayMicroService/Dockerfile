#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/APIGatewayMicroService/APIGateway.csproj", "src/APIGatewayMicroService/"]
# Copy the NuGet.config file
COPY NuGetDocker.config /root/.nuget/NuGetDocker.config

# Copy the local NuGet packages to the Docker image
COPY /src/Shared/SharedOutput/ /root/.nuget/packages/
COPY /packages/ /root/.nuget/packages/

#RUN dotnet restore "./src/APIGatewayMicroService/APIGateway.csproj"
RUN dotnet restore "./src/APIGatewayMicroService/APIGateway.csproj" --configfile /root/.nuget/NuGetDocker.config
COPY . .
WORKDIR "/src/src/APIGatewayMicroService"
RUN dotnet build "./APIGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./APIGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
# Copy the development certificate
COPY /certs/aspnetcore-devcert.pfx app/certs/aspnetcore-devcert.pfx
# Expose port 5001 for HTTPS
EXPOSE 8081
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APIGateway.dll"]