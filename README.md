# TmdbBully

> This little app is meant to fetch top 120 movies from TMDB's API, either on-demand or every X seconds by setting up a CRON job.

Before building and running this project, make sure to add you TMDB API key and DB connection string to appsettings.json:
```json
"ApiKey": "YOUR-API-KEY",
"ConnectionStrings": {
  "DbConnectionString": "YOUR-CONNECTION-STRING"
}
```

The project also includes a `CreateTables.sql` script which helps you create the tables in your SQL Server database.

When you run the app, it will prompt you to a Swagger UI interface, which guides you through all the available endpoints.
