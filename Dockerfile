# Utiliza la imagen oficial de .NET para construir y publicar la app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["VehicleAPI.csproj", "."]
RUN dotnet restore "./VehicleAPI.csproj"
COPY . .
RUN dotnet publish "VehicleAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "VehicleAPI.dll"]
