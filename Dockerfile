# Use the official .NET 9 SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY LibraryApp/LibraryApp.csproj ./LibraryApp/
RUN dotnet restore ./LibraryApp/LibraryApp.csproj

# Copy the rest of the source code
COPY . .

# Publish the application to the /app/publish directory
RUN dotnet publish ./LibraryApp/LibraryApp.csproj -c Release -o /app/publish

# Use the official .NET 9 ASP.NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# Expose port 80 for the web server
EXPOSE 80
EXPOSE 8080

# Set environment variable for ASP.NET Core
ENV ASPNETCORE_URLS=http://+:80

# Start the application
ENTRYPOINT ["dotnet", "LibraryApp.dll"]

