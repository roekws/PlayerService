<a id="readme-top"></a>

<h3 align="center">Player Service</h3>

<p align="center">
  Player service for Dota 2 custom game
</p>

<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#ci-workflow">CI Workflow</a></li>
    <li><a href="#license">License</a></li>
  </ol>
</details>

## About

A backend service for managing player's character data.

### Project Structure

- src/Players.AppHost - The .NET project that orchestrates the app model
- src/Players.ServiceDefaults:
  - Configures OpenTelemetry metrics and tracing.
  - Adds default health check endpoints.
  - Adds service discovery functionality.
  - Configures HttpClient to work with service discovery.

- src/Players.API - Web API for player data management
- src/Players.Core - Domain classes, context and migrations
- src/Players.Tests - tests


<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With
* NET Aspire - cloud ready stack for building  applications
* ASP.NET Core - web framework
* EntityFramework Core - Object-Relational Mapper (ORM) for .NET
* Npgsql - .NET Access to PostgreSQL

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Getting Started

Setting up project locally.

### Prerequisites

* Git
* .NET SDK 9
* PostgreSQL
* Docker

### Installation

1. You need to trust the ASP.NET Core localhost certificate before running the app. Run the following command:
   ```sh
    dotnet dev-certs https --trust
   ```

2. Build and start:
   ```sh
   dotnet run --project .\src\Players.AppHost\
   ```

3. Access the API Documentation at: https://localhost:7208/scalar

To create new migration:
```sh
dotnet ef migrations add NewMigration --project src/Players.Core --startup-project src/Players.API
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

# CI Workflow

CI workflow performs runs automatically on push to master:

- Builds and tests .NET server.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## License

Distributed under the project_license. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>
