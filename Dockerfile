# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and restore
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/out .

# Expose the port your app will run on
EXPOSE 5000

# Set ASP.NET Core to listen on port 5000
ENV ASPNETCORE_URLS=http://0.0.0.0:5000

# Replace this DLL name with YOUR actual .dll from your project
ENTRYPOINT ["dotnet", "ToolCaptureAppClean.dll"]

