FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

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