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
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#ci-workflow">CI Workflow</a></li>
    <li><a href="#license">License</a></li>
  </ol>
</details>

## About

A backend service for managing player's character data.

### Project Structure

- src/PlayerService.API - Web API for player data management
- src/PlayerService.Core - Domain classes
- src/PlayerService.Tests - tests


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

### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/roekws/PlayerService
   ```
3. Restore .NET Dependencies
   ```sh
   dotnet restore ./src
   ```
4. Build .NET
   ```sh
   dotnet build ./src
   ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Roadmap

- [ ] Store player's characters
- [ ] Save characters data
- [ ] Get characters data
- ...
- [ ] Key validation

<p align="right">(<a href="#readme-top">back to top</a>)</p>

# CI Workflow

CI workflow performs runs automatically on push to master:

- Builds and tests .NET server.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## License

Distributed under the project_license. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>