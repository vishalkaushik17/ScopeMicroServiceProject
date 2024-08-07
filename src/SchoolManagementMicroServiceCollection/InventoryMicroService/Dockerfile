#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SchoolManagementMicroServiceCollection/InventoryMicroService/InventoryMicroService.csproj", "src/SchoolManagementMicroServiceCollection/InventoryMicroService/"]
COPY ["src/Shared/CommonLayerLibrary/GenericFunction.csproj", "src/Shared/CommonLayerLibrary/"]
COPY ["src/Shared/CommonServiceLayer/SharedLibrary.csproj", "src/Shared/CommonServiceLayer/"]
COPY ["src/Shared/DataLayerLibrary/DatabaseLibrary/DatabaseMigrationService/DBOperationsLayer.csproj", "src/Shared/DataLayerLibrary/DatabaseLibrary/DatabaseMigrationService/"]
COPY ["src/Shared/ModelLayerLibrary/DataModelDtoModelLayer/ModelTemplates.csproj", "src/Shared/ModelLayerLibrary/DataModelDtoModelLayer/"]
COPY ["src/Shared/DataLayerLibrary/CacheDataLibrary/DataCacheLayer.csproj", "src/Shared/DataLayerLibrary/CacheDataLibrary/"]
COPY ["src/Shared/ModelLayerLibrary/ViewModelLayer/VModelLayer.csproj", "src/Shared/ModelLayerLibrary/ViewModelLayer/"]
COPY ["src/Shared/IdentityLayerLibrary/IdentityLayer.csproj", "src/Shared/IdentityLayerLibrary/"]
COPY ["src/Shared/DataLayerLibrary/DatabaseLibrary/DatabaseService/DataBaseServices.csproj", "src/Shared/DataLayerLibrary/DatabaseLibrary/DatabaseService/"]
COPY ["src/Shared/UnitOfWorkLayerLibrary/UnitOfWork.csproj", "src/Shared/UnitOfWorkLayerLibrary/"]
COPY ["src/Shared/BusinessLayerLibrary/BSLayerSchool/BSLayerSchool.csproj", "src/Shared/BusinessLayerLibrary/BSLayerSchool/"]
COPY ["src/Shared/CacheLayerLibrary/MCCommonLayer/MCCommonLayer.csproj", "src/Shared/CacheLayerLibrary/MCCommonLayer/"]
COPY ["src/Shared/CacheLayerLibrary/MCEmployeeLayer/MCEmployeeLayer.csproj", "src/Shared/CacheLayerLibrary/MCEmployeeLayer/"]
COPY ["src/Shared/CacheLayerLibrary/MCInventoryLayer/MCInventoryLayer.csproj", "src/Shared/CacheLayerLibrary/MCInventoryLayer/"]
COPY ["src/SchoolManagementMicroServiceCollection/CommonMicroService/CommonMicroService.csproj", "src/SchoolManagementMicroServiceCollection/CommonMicroService/"]
COPY ["src/AuthenticationMicroService/AuthenticateService.csproj", "src/AuthenticationMicroService/"]
COPY ["src/Shared/BusinessLayerLibrary/BSAuthentication/BSAuthentication.csproj", "src/Shared/BusinessLayerLibrary/BSAuthentication/"]
COPY ["src/Shared/DILayerLibrary/DependencyInjection.csproj", "src/Shared/DILayerLibrary/"]
COPY ["src/Shared/BusinessLayerLibrary/BSCodingCompany/BSCodingCompany.csproj", "src/Shared/BusinessLayerLibrary/BSCodingCompany/"]
RUN dotnet restore "./src/SchoolManagementMicroServiceCollection/InventoryMicroService/InventoryMicroService.csproj"
COPY . .
WORKDIR "/src/src/SchoolManagementMicroServiceCollection/InventoryMicroService"
RUN dotnet build "./InventoryMicroService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InventoryMicroService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InventoryMicroService.dll"]