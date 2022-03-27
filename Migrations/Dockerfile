FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build

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