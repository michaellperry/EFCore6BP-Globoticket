FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build

# Install EF tool
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Restore packages
WORKDIR /source

COPY GloboTicket.API/*.csproj             ./GloboTicket.API/
COPY GloboTicket.Domain/*.csproj          ./GloboTicket.Domain/
COPY GloboTicket.Infrastructure/*.csproj  ./GloboTicket.Infrastructure/
COPY GloboTicket.SharedKernel/*.csproj    ./GloboTicket.SharedKernel/
RUN dotnet restore GloboTicket.API/GloboTicket.API.csproj

# Build the projects
COPY GloboTicket.API/             ./GloboTicket.API/
COPY GloboTicket.Domain/          ./GloboTicket.Domain/
COPY GloboTicket.Infrastructure/  ./GloboTicket.Infrastructure/
COPY GloboTicket.SharedKernel/    ./GloboTicket.SharedKernel/
RUN dotnet build GloboTicket.API/GloboTicket.API.csproj -c Release --no-restore

# Build the migration bundle
ENV GLOBOTICKET_ADMIN_CONNECTION_STRING="Server=tcp:mssql;Database=GloboTicket;User=sa;Password=notused;TrustServerCertificate=True;"
RUN dotnet ef migrations bundle -p ./GloboTicket.Infrastructure -s ./GloboTicket.API --context GloboTicket.Infrastructure.Configuration.SqlGloboTicketContext --configuration Release --no-build --self-contained

FROM mcr.microsoft.com/dotnet/runtime:8.0-jammy AS run

# Install tool to convert line endings
RUN apt-get update && apt-get install -y dos2unix

WORKDIR /home

COPY --from=build /source/efbundle .

COPY GloboTicket.API/appsettings.json .
COPY Migrations/run_migrations.sh .

# Convert line endings on the script
RUN dos2unix run_migrations.sh

ENV GLOBOTICKET_ADMIN_CONNECTION_STRING="Server=tcp:mssql;Database=GloboTicket;User=sa;Password=notused;TrustServerCertificate=True;"

ENTRYPOINT ["./run_migrations.sh"]