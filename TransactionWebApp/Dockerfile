# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project file and restore any dependencies (via nuget)
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application files
COPY . ./

# Publish the application in release mode to the /app folder
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

# Set the working directory inside the container
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Set the environment variable to indicate the app should run in production
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose the port the app will run on (default is 5000 for .NET Core)
EXPOSE 5000

# Define the command to run the app
ENTRYPOINT ["dotnet", "TransactionWebApp.dll"]
