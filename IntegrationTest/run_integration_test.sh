#!/bin/bash
set -e

# Run the migrations
GLOBOTICKET_ADMIN_CONNECTION_STRING="Server=tcp:mssql;Database=GloboTicket;User=sa;Password=${GLOBOTICKET_ADMIN_PASSWORD};TrustServerCertificate=True;"
./efbundle --connection $GLOBOTICKET_ADMIN_CONNECTION_STRING

# Create the application user
sqlcmd -S mssql -d GloboTicket -U sa -P $GLOBOTICKET_ADMIN_PASSWORD -C <<-EOSQL
    CREATE LOGIN app WITH PASSWORD='${GLOBOTICKET_APP_PASSWORD}';
    CREATE USER app FOR LOGIN app;
    ALTER ROLE globoticket_app ADD MEMBER app;
EOSQL

# Run the integration tests
export GLOBOTICKET_APP_CONNECTION_STRING="Server=tcp:mssql;Database=GloboTicket;User=app;Password=${GLOBOTICKET_APP_PASSWORD};TrustServerCertificate=True;"
dotnet test --no-build -c Release ./GloboTicket.IntegrationTest