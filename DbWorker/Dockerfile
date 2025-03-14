# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
WORKDIR /app
# Copy .csproj and restore dependencies
COPY Api/*.csproj ./Api/
WORKDIR /app/Api
RUN dotnet restore


# Install curl as an example
RUN apk add --no-cache curl

# Copy everything else and build the application
WORKDIR /app
COPY Api/. ./Api/
RUN dotnet publish Api -c Release -o /app/publish

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine

# Add a non-root user to run the application
RUN addgroup -S appgroup && adduser -S appuser -G appgroup

USER appuser:appgroup

WORKDIR /app

# Copy published output from build stage
COPY --from=build-env /app/publish .

ENTRYPOINT ["dotnet", "Api.dll"]
