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