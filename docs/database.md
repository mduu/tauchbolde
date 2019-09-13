# Tauchbolde Database Development

We use Entity Framework Core to manage and access our database. Currently we run on Sqlite.

## Add a database migration

1. Change the model code as needed.
1. Open a terminal and navigate into the `/src/Tauchbolde.DataAccess` folder.
1. Execute the command: `dotnet ef migrations add <MIGRATION_NAME> -s ../Tauchbolde.Web/`
1. Review the generated code.
1. Update the database as a final test.

## Update database

1. Open a terminal and navigate into the `/src/Tauchbolde.DataAccess` folder.
1. Execute the command: `dotnet ef database update -s ../Tauchbolde.Web`.

## Create SQL migration scrip

1. Open a terminal and navigate to `/src/Tauchbolde.DataAccess` folder.
1. Execute: `dotnet ef migrations script -i  -o ../../sql/update_to_latest.sql -s ../Tauchbolde.Web`.
