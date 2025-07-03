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
      <li>
        <a href="#development">Development</a>
        <ul>
        <li><a href="#testing">Testing</a></li>
      </ul>
      </li>
      <li><a href="#sources">Sources</a></li>
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

- src/Players.API - Web API for handling http requests
- src/Players.Core - Domain classes, database context, services for player data management and migrations
- src/Players.Tests - tests

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With
* ASP.NET Core - web framework
* EntityFramework Core - Object-Relational Mapper (ORM) for .NET
* Npgsql - .NET Access to PostgreSQL
* Docker - Containerization platform

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

1. Create a .env.dev file in the project root with these variables (example values):
  ```sh
  DB_NAME=dbname
  DB_USER=dbuser
  DB_PASSWORD=dbpassword
  SERVER_KEY=serverkey
  ADMIN_KEY=adminkey
  ASPNETCORE_ENVIRONMENT=Development
  K6_WEB_DASHBOARD=true
  ```

2. Build and start:
  ```sh
  docker compose --env-file .env.dev up --build
  ```

3. Access OpenApi Doc and Interactive API Doc at:
  ```sh
  http://127.0.0.1:8080/openapi/v1.json
  http://127.0.0.1:8080/scalar/
  ```

4. Stop with:
  ```sh
  docker compose down
  ```

5. Develop with:
  ```sh
  docker compose watch --env-file .env.dev
  ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Development

### Testing

Load Testing:

1. Pull docker image k6 with:
  ```sh
  docker pull grafana/k6
  ```

2. Run tests:
  ```sh
  docker run --rm `
  --env-file .env.dev `
  -p 5665:5665 `
  -v ${PWD}/tests/Load:/scripts `
  grafana/k6 run `
  --out csv=/scripts/results/k6_results.csv `
  /scripts/k6-script.test.ts
  ```
  to simulate virtual users add
  ```sh
  --vus 100 --duration 30s
  ```

3. Results:

  - Console output: Printed after test completion
  - UI Dashboard: Access at
  ```
  http://127.0.0.1:5665/
  ```

To extend load tests:

1. Update OpenAPI Spec:
  ```sh
  curl -o ./tests/Load/v1.json http://127.0.0.1:8080/openapi/v1.json
  ```
  Or download manually at:
  ```sh
  http://127.0.0.1:8080/openapi/v1.json
  ```

2. Regenerate Client:
  ```sh
  npx @grafana/openapi-to-k6 ./tests/Load/v1.json ./
  ```

3. Add/Update tests in:
  ```sh
  ./tests/Load/k6-script.test.ts
  ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Sources

- [API Key Authentication](https://habr.com/ru/articles/877302/)
- [Claims-based authorization in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/claims?view=aspnetcore-9.0)
- [.NET Docker](https://docs.docker.com/guides/dotnet/)
- [Grafana k6 load testing tool](https://grafana.com/docs/k6/latest/)
- [(CI) Workflow for Building and testing .NET](https://docs.github.com/en/actions/how-tos/use-cases-and-examples/building-and-testing/building-and-testing-net)
- [(CI) Storing and sharing data from a workflow](https://docs.github.com/en/actions/how-tos/writing-workflows/choosing-what-your-workflow-does/storing-and-sharing-data-from-a-workflow)

<p align="right">(<a href="#readme-top">back to top</a>)</p>
