FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Install sqlcmd
ENV ACCEPT_EULA=Y
RUN apt update
RUN apt install -y gnupg2
RUN curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -
RUN curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list | tee /etc/apt/sources.list.d/msprod.list
RUN apt update
RUN apt install -y mssql-tools
ENV PATH="${PATH}:/opt/mssql-tools/bin"

# Install EF tool
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Install tool to convert line endings
RUN apt-get update && apt-get install -y dos2unix

# Restore packages
WORKDIR /source

COPY GloboTicket.sln .
COPY GloboTicket.API/*.csproj             ./GloboTicket.API/
COPY GloboTicket.Domain/*.csproj          ./GloboTicket.Domain/
COPY GloboTicket.Infrastructure/*.csproj  ./GloboTicket.Infrastructure/
COPY GloboTicket.IntegrationTest/*.csproj ./GloboTicket.IntegrationTest/
COPY GloboTicket.SharedKernel/*.csproj    ./GloboTicket.SharedKernel/
COPY GloboTicket.UnitTest/*.csproj        ./GloboTicket.UnitTest/
RUN dotnet restore --runtime linux-x64

# Build and unit test
COPY GloboTicket.API/             ./GloboTicket.API/
COPY GloboTicket.Domain/          ./GloboTicket.Domain/
COPY GloboTicket.Infrastructure/  ./GloboTicket.Infrastructure/
COPY GloboTicket.IntegrationTest/ ./GloboTicket.IntegrationTest/
COPY GloboTicket.SharedKernel/    ./GloboTicket.SharedKernel/
COPY GloboTicket.UnitTest/        ./GloboTicket.UnitTest/
RUN dotnet build -c Release --no-restore
RUN dotnet test --no-build -c Release ./GloboTicket.UnitTest

# Build the migration bundle
ENV GLOBOTICKET_ADMIN_CONNECTION_STRING="Server=tcp:mssql;Database=GloboTicket;User=sa;Password=notused;TrustServerCertificate=True;"
RUN dotnet ef migrations bundle -p ./GloboTicket.Infrastructure -s ./GloboTicket.API --configuration Release --no-build
RUN cp ./GloboTicket.API/appsettings.json .

# Run the integration tests
COPY IntegrationTest/run_integration_test.sh .
RUN chmod +x ./run_integration_test.sh

# Convert line endings on the script
RUN dos2unix ./run_integration_test.sh

ENTRYPOINT ["./run_integration_test.sh"]