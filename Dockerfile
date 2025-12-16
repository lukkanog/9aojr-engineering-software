# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ProductProcessing/ProductProcessing.csproj ProductProcessing/
RUN dotnet restore ProductProcessing/ProductProcessing.csproj

# Copy everything else and build
COPY ProductProcessing/ ProductProcessing/
WORKDIR /src/ProductProcessing
RUN dotnet build ProductProcessing.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish ProductProcessing.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductProcessing.dll"]
