# Pull DOTNET 6 SDK from microsoft registry
FROM mcr.microsoft.com/dotnet/sdk:6.0 as BUILD_DEVELOPMENT

# Set a name for the image 
LABEL name="opencommercecore_api"

# Copy project
COPY . /tmp/openecommercecore/
WORKDIR /tmp/openecommercecore/

# Build the application
RUN dotnet restore
RUN dotnet publish -c Release -r linux-x64 --self-contained -o /opt/openecommercecore

# PULL ASPNET 6 Runtime from Microsoft Registry
FROM mcr.microsoft.com/dotnet/runtime:6.0 as RUNTIME

WORKDIR /opt/openecommercecore
COPY --from=BUILD_DEVELOPMENT /opt/openecommercecore .

ENV ASPNETCORE_ENVIRONMENT="Production"

# Set Entrypoint to run application
ENTRYPOINT ["./Core.API" ]