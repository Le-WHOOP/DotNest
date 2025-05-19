FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY DotNest.csproj ./
# Restore dependencies
RUN dotnet restore

# Copy everything
COPY ./ ./
# Build and publish a release
RUN dotnet publish -c Release --property:PublishDir=out

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /app
# Copy the executable
COPY --from=build /src/out/ ./
EXPOSE 8080
ENTRYPOINT [ "dotnet", "DotNest.dll" ]
