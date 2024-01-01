# Pull DOTNET 6 SDK from microsoft registry
FROM mcr.microsoft.com/dotnet/sdk:6.0 as BUILD_DEVELOPMENT

# Copy project
COPY . /tmp/openecommercecore/
WORKDIR /tmp/openecommercecore/

# Build the application
RUN dotnet restore
RUN dotnet publish -c Debug -r linux-x64 -o /opt/openecommercecore

# PULL ASPNET 6 Runtime from Microsoft Registry
FROM mcr.microsoft.com/dotnet/runtime:6.0 as RUNTIME

WORKDIR /opt/openecommercecore
COPY --from=BUILD_DEVELOPMENT /opt/openecommercecore .

ENV ASPNETCORE_ENVIRONMENT="Development"

# Set Entrypoint to run application
ENTRYPOINT ["./Core.API" ]