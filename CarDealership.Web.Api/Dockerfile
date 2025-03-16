FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CarDealership.Web.Api/CarDealership.Web.Api.csproj", "CarDealership.Web.Api/"]
COPY ["CarDealership.Application/CarDealership.Application.csproj", "CarDealership.Application/"]
COPY ["CarDealership.Core/CarDealership.Core.csproj", "CarDealership.Core/"]
COPY ["CarDealership.DataAccess/CarDealership.DataAccess.csproj", "CarDealership.DataAccess/"]
COPY ["CarDealership.Infrastructure/CarDealership.Infrastructure.csproj", "CarDealership.Infrastructure/"]
RUN dotnet restore "./CarDealership.Web.Api/./CarDealership.Web.Api.csproj"
COPY . .
WORKDIR "/src/CarDealership.Web.Api"
RUN dotnet build "./CarDealership.Web.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarDealership.Web.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarDealership.Web.Api.dll"]