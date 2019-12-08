# EF Core SQLite

SQLite has an EF Core provider, which is great, because SQLite is a RDBMS which
can run in-process unlike SQL Server, Postgres etc. but still has on-disk
persistence unlike say the in-memory provider. Unfortunately, in SQLite, a lot
of schema operations require table rebuilds, which are not supported in EF Core:

https://github.com/aspnet/EntityFrameworkCore/issues/329

This repository demonstrates the problem.

- `dotnet new console`
- `dotnet add package Microsoft.EntityFrameworkCore.Sqlite`
- Create `AppDbContext` with a single `DbSet` property of items
- Create the `Item` model class with an ID and a name
- Override `OnConfiguring` in `AppDbContext` to connect to SQLite
- Reset the database and then insert, save and retrieve an item for a test
  - Use `EnsureDeleted` following by `EnsureCreated` to carry out the reset
- `dotnet run` to verify everything works
- Reference the design package for EF tools:
  `dotnet add package Microsoft.EntityFrameworkCore.Design`
- Scaffold the initial migration:
  `dotnet ef migrations add ScaffoldInitialSchema`
- Create a new entity - `User` and reference it in an `Item` and vice-versa 1:N
- Create a new `DbSet` for the user entity
- Expand the demo code to create the user with the item and persist both at once
- Scaffold a new migration capturing the new entity and relationships:
  `dotnet ef migrations add AddUserEntityAndItemReference`
- Replace the database reset with a `Migrate` call to force automated migration
  - Or not replace but follow up?

## To-Do

### Update to demonstrate the table rebuild issue

I failed to demonstrate the table rebuild issue I faced in my `habiter` repo.
I need to update this and then ask in the GitHub issue at
https://github.com/aspnet/EntityFrameworkCore/issues/329
whether the comment about column renames means the migration I was attempting
there will now work.
