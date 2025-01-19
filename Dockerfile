# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /app

# Copy the solution file and project files to the container
COPY *.cspros ./  # Copies the solution file
COPY Langua.WebUI/Langua.WebUI.csproj ./Langua.WebUI/

# Restore dependencies
RUN dotnet restore

# Copy the entire source code
COPY . .

# Set the working directory to the main project
WORKDIR /src/Langua.WebUI

# Build and publish the application
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET Runtime image to run the application
FROM mcr.microsoft.com/dotnet/runtime:8.0

# Set the working directory for the runtime image
WORKDIR /app

# Copy the build output from the SDK image to the runtime image
COPY --from=build /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "Langua.WebUI.dll"]
