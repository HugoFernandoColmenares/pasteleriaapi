# Usar la imagen base de .NET 8 SDK para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos de solución y restaurar las dependencias
COPY *.sln .
COPY Pasteleria.Api/*.csproj ./Pasteleria.Api/
COPY Pasteleria.Data/*.csproj ./Pasteleria.Data/
COPY Pasteleria.Shared/*.csproj ./Pasteleria.Shared/
COPY Pasteleria.Business/*.csproj ./Pasteleria.Business/
RUN dotnet restore

# Copiar el resto del código y compilar la aplicación
COPY . .
WORKDIR /src/Pasteleria.Api
RUN dotnet build -c Release -o /app/build

# Publicar la aplicación
RUN dotnet publish -c Release -o /app/publish

# Usar la imagen base de .NET 8 Runtime para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados desde la etapa de compilación
COPY --from=build /app/publish .

# Exponer el puerto en el que la aplicación escucha
EXPOSE 5000

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "Pasteleria.Api.dll"]
