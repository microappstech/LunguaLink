
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY *.csproj ./

RUN dotnet restore

# Copy the entire source code
COPY . .

# Set the working directory to the main project
WORKDIR /src/Langua.WebUI

# Build and publish the application
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0

WORKDIR /app

# Copy the build output from the SDK image to the runtime image
COPY --from=build /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "Langua.WebUI.dll"]
