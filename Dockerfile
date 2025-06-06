# Imagen base de ASP.NET 9 para producción
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Imagen SDK de .NET 9 para compilar y publicar
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia el archivo de proyecto y restaura dependencias
COPY ["RegistroTecnicoPostgre.csproj", "./"]
RUN dotnet restore "RegistroTecnicoPostgre.csproj"

# Copia el resto de los archivos y publica
COPY . .
RUN dotnet publish "RegistroTecnicoPostgre.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final para ejecución
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "RegistroTecnicoPostgre.dll"]