## Create and Run a Migrations Bundle

To create a migrations bundle image:

```
docker build -t globoticket/migrations:latest -f ./Migrations/Dockerfile .
```

To shell in:

```
docker run --name globoticket-migrations -it --rm --entrypoint /bin/bash globoticket/migrations:latest
```

To run migrations:

```
docker run --name globoticket-migrations -it --rm globoticket/migrations:latest -e GLOBOTICKET_ADMIN_CONNECTION_STRING=Server=tcp:mssql;Database=GloboTicket;User=sa;Password=notused;TrustServerCertificate=True;
```

## Create a Release Pipeline

Show your ACR credentials:

```
az login
az account set --subscription=<your subscription ID>
az acr credential show -n mlpgloboticket
```

Add a step to your Release pipeline.
- Task: Azure CLI
- Service Connection: Your Azure service connection
- Script Type: PowerShell Core
- Inline Script:

```
az container create `
  -n globoticket-migration -g EFCore6BP `
  -e GLOBOTICKET_ADMIN_CONNECTION_STRING="$(GLOBOTICKET_ADMIN_CONNECTION_STRING)" `
  --image mlpgloboticket.azurecr.io/globoticket-migration:$(Build.BuildNumber) `
  --registry-username mlpgloboticket `
  --registry-password $(RegistryPassword)

az container attach `
  -n globoticket-migration -g EFCore6BP

az container delete `
  -n globoticket-migration -g EFCore6BP `
  --yes
```

Set pipeline variables:
- GLOBOTICKET_ADMIN_CONNECTION_STRING: Admin connection string for the target environment
- RegistryPassword: Password obtained from credential show above