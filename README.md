## One-Time Developer Setup

This project demonstrates separation of concerns and shifting security left.
As a result some special developer setup will be required.

### Configure the Admin Connection String

First, configure a connection string for running migrations.
This connection must have owner privileges.
If you are on Windows, you can use a trusted connection.

```powershell
$env:GLOBOTICKET_ADMIN_CONNECTION_STRING='Server=(local);Initial Catalog=GloboTicket;Integrated Security=True;Trust Server Certificate=True;'
```

If you are not using Windows authentication in your local database (for example, you are working on a Mac), then generate a secure password.

```bash
export $GLOBOTICKET_ADMIN_CONNECTION_STRING='Server=.;Initial Catalog=GloboTicket;User=sa;Password=adminpassword;Trust Server Certificate=True;'
```

### Create an Application User

Run the migrations to create the GloboTicket database:

```powershell
dotnet ef database update -p .\GloboTicket.Infrastructure -s .\GloboTicket.API
```

Then go to SSMS to create an application login.
Generate a new password.
This password will only be used on your development workstation.

```sql
CREATE LOGIN app WITH PASSWORD='applicationpassword'
```

Add a user to the GloboTicket database for this login.

```sql
USE GloboTicket

CREATE USER app FOR LOGIN app
```

Assign the user to the `globoticket_app` role so that it can read and write to the database.

```sql
USE GloboTicket

ALTER ROLE globoticket_app ADD MEMBER app
```

### Configure the Application Connection String

The application connection string is different for each developer.
We therefore store it in User Secrets.
Right-click on the `GloboTicket.API` project in Visual Studio and select "Manage User Secrets".
If you work in Visual Studio Code, you can install the [.NET Core User Secrets](https://marketplace.visualstudio.com/items?itemName=adrianwilczynski.user-secrets) extension.

Paste the following into `secrets.json` and set your password:

```json
{
  "ConnectionStrings": {
    "GloboTicketConnection": "Server=(local);Initial Catalog=GloboTicket;User=app;Password=applicationpassword;MultipleActiveResultSets=True;Trust Server Certificate=True;"
  }
}
```

## Update your Developer Database

Every time you pull code that might include new migrations, run the database update command:

```powershell
dotnet ef database update -p .\GloboTicket.Infrastructure -s .\GloboTicket.API
```

To define a new migration, run the following command to add it to the Infrastructure project:

```powershell
dotnet ef migrations add -p .\GloboTicket.Infrastructure -s .\GloboTicket.API MyNewMigration
```

Entity mappings belong in the Domain project.
Create a class that implements the `IEntityTypeConfiguration<T>` interface in the Configuration folder.
You only have access to general entity mappings, nothing SQL-Server specific.

Occasionally you will need to add SQL-Server specific configuration.
These belong in the Infrastructure project.
Create a class that implements the `IEntityTypeConfiguration<T>` interface in the Configuration folder.
This gives you access to SQL-Server configurations.

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
docker run --name globoticket-migrations -it --rm globoticket/migrations:latest -e GLOBOTICKET_ADMIN_CONNECTION_STRING="Server=tcp:mssql;Database=GloboTicket;User=sa;Password=notused;TrustServerCertificate=True;"
```
