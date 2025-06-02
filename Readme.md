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
        <li><a href="#sources">Sources</a></li>
      </ul>
    </li>
  </ol>
</details>

## About

A backend service for managing game data.

Game loop:
- Pick hero, skills, base structures
- Battle against same strength bases

[Database structure](https://github.com/roekws/PlayerService/raw/master/Documentation/db.png "Db")

### Project Structure

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
  ADMIN_KEY=adminkey
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

5. Develop with:
    ```sh
   docker compose watch
   ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Sources

- [API Key Authentication](https://habr.com/ru/articles/877302/)
- [Claims-based authorization in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/claims?view=aspnetcore-9.0)
- [.NET Docker](https://docs.docker.com/guides/dotnet/)
- []()

<p align="right">(<a href="#readme-top">back to top</a>)</p>
