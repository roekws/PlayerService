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

0. (Optional) To create new migration:
  ```sh
  dotnet ef migrations add NewMigration --project src/Players.Core --startup-project src/Players.API
  ```

1. Create a .env file in the project root with these variables (example values):
  ```sh
  DB_NAME=dbname
  DB_USER=dbuser
  DB_PASSWORD=dbpassword
  SERVER_KEY=serverkey
  PUBLIC_KEY=publickey
  ASPNETCORE_ENVIRONMENT=Development
  ```

2. Build and start:
   ```sh
   docker-compose up --build
   ```

3. Access the API at: http://127.0.0.1:8080/scalar/

4. Stop with:
   ```sh
   docker-compose down
   ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### CI Workflow

CI workflow performs runs automatically on push to master:

- Builds .NET server.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### License

Distributed under the project_license. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>
