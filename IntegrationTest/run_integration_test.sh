#!/bin/bash
set -e

# Run the migrations
GLOBOTICKET_ADMIN_CONNECTION_STRING="Server=tcp:mssql;Database=GloboTicket;User=sa;Password=${GLOBOTICKET_ADMIN_PASSWORD};TrustServerCertificate=True;"
./efbundle --connection $GLOBOTICKET_ADMIN_CONNECTION_STRING