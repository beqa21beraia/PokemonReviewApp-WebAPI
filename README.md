<div align="center">

# ⚡ PokemonReviewApp

**A clean RESTful Web API for managing Pokémon, owners, categories, countries, reviewers, and reviews — built with ASP.NET Core 8, Entity Framework Core, SQL Server, AutoMapper, Swagger, and Serilog.**

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat-square&logo=csharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-512BD4?style=flat-square&logo=dotnet)](https://learn.microsoft.com/en-us/aspnet/core/web-api/)
[![EF Core](https://img.shields.io/badge/Entity%20Framework-Core-512BD4?style=flat-square&logo=dotnet)](https://learn.microsoft.com/en-us/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?style=flat-square&logo=microsoftsqlserver)](https://www.microsoft.com/en-us/sql-server)
[![Swagger](https://img.shields.io/badge/API%20Docs-Swagger%20UI-85EA2D?style=flat-square&logo=swagger)](https://swagger.io/)
[![Serilog](https://img.shields.io/badge/Logging-Serilog-2D2D2D?style=flat-square)](https://serilog.net/)
[![AutoMapper](https://img.shields.io/badge/Mapping-AutoMapper-EF4B4B?style=flat-square)](https://automapper.org/)
[![Repository Pattern](https://img.shields.io/badge/Architecture-Repository%20Pattern-blue?style=flat-square)](https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/models-data/repository-pattern-cs)
[![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Linux-lightgrey?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)

[📁 Source Code](https://github.com/beqa21beraia/PokemonReviewApp) · [🐛 Report an Issue](https://github.com/beqa21beraia/PokemonReviewApp/issues)

</div>

---

## 📖 Overview

**PokemonReviewApp** is a backend REST API built with **ASP.NET Core 8 Web API**.  
It allows users to manage Pokémon-related data such as Pokémon, categories, countries, owners, reviewers, and reviews.

The project was built as a learning-focused backend application to practice real-world API development concepts such as:

- Clean controller-based API structure
- Entity Framework Core with SQL Server
- Repository pattern for data access abstraction
- DTOs for safer API request/response models
- AutoMapper for object mapping
- Async/await for asynchronous database operations
- Swagger UI for API documentation and testing
- Serilog for structured logging
- EF Core migrations and database seeding

---

## ✨ Key Features

- **Full CRUD operations** for Pokémon, categories, countries, owners, reviewers, and reviews
- **Pokémon rating calculation** based on related reviews
- **Relationship-based queries**, such as:
  - Get Pokémon by category
  - Get Pokémon by owner
  - Get reviews for a Pokémon
  - Get reviews by reviewer
  - Get country by owner
- **Repository Pattern** for cleaner separation between controllers and database logic
- **DTO-based API design** to avoid exposing domain models directly
- **AutoMapper integration** for model-to-DTO mapping
- **SQL Server database** managed with Entity Framework Core migrations
- **Structured logging** with Serilog console and rolling file logs
- **Swagger UI** for testing endpoints directly from the browser

---

## 🗂 Project Structure

```bash
PokemonReviewApp/
│
├── PokemonReviewApp/                  # Main ASP.NET Core Web API project
│   │
│   ├── Controllers/                   # API controllers
│   │   ├── CategoryController.cs
│   │   ├── CountryController.cs
│   │   ├── OwnerController.cs
│   │   ├── PokemonController.cs
│   │   ├── ReviewController.cs
│   │   └── ReviewerController.cs
│   │
│   ├── DTO/                           # Data Transfer Objects
│   │   ├── CategoryDto.cs
│   │   ├── CountryDto.cs
│   │   ├── OwnerDto.cs
│   │   ├── PokemonDto.cs
│   │   ├── ReviewDto.cs
│   │   └── ReviewerDto.cs
│   │
│   ├── Data/
│   │   └── DataContext.cs             # EF Core DbContext
│   │
│   ├── Helper/
│   │   └── MappingProfiles.cs         # AutoMapper configuration
│   │
│   ├── Interfaces/                    # Repository contracts
│   │   ├── ICategoryRepository.cs
│   │   ├── ICountryRepository.cs
│   │   ├── IOwnerRepository.cs
│   │   ├── IPokemonRepository.cs
│   │   ├── IReviewRepository.cs
│   │   └── IReviewerRepository.cs
│   │
│   ├── Migrations/                    # EF Core migrations
│   │
│   ├── Models/                        # Domain entities
│   │   ├── Category.cs
│   │   ├── Country.cs
│   │   ├── Owner.cs
│   │   ├── Pokemon.cs
│   │   ├── PokemonCategory.cs
│   │   ├── PokemonOwner.cs
│   │   ├── Review.cs
│   │   └── Reviewer.cs
│   │
│   ├── Repository/                    # Repository implementations
│   │   ├── CategoryRepository.cs
│   │   ├── CountryRepository.cs
│   │   ├── OwnerRepository.cs
│   │   ├── PokemonRepository.cs
│   │   ├── ReviewRepository.cs
│   │   └── ReviewerRepository.cs
│   │
│   ├── Properties/
│   ├── Logs/                          # Auto-generated Serilog log files
│   ├── Program.cs                     # App startup and service registration
│   ├── Seed.cs                        # Database seeding logic
│   ├── appsettings.json               # App configuration
│   └── PokemonReviewApp.csproj        # Project file and dependencies
│
├── PokemonReviewApp.sln               # Solution file
├── .editorconfig
├── .gitignore
└── README.md
````

---

## 🛠 Tech Stack

| Concern              | Technology                   |
| -------------------- | ---------------------------- |
| Framework            | ASP.NET Core 8 Web API       |
| Language             | C#                           |
| Database             | SQL Server                   |
| ORM                  | Entity Framework Core        |
| API Documentation    | Swagger / Swashbuckle        |
| Mapping              | AutoMapper                   |
| Logging              | Serilog                      |
| Architecture Pattern | Repository Pattern           |
| Data Models          | DTOs + Domain Models         |
| Database Management  | EF Core Migrations           |
| IDE                  | Visual Studio 2022 / VS Code |

---

## 🧩 Domain Model

The API is built around the following main entities:

| Entity            | Purpose                                                  |
| ----------------- | -------------------------------------------------------- |
| `Pokemon`         | Represents a Pokémon with name and birth date            |
| `Category`        | Represents Pokémon categories/types                      |
| `Country`         | Represents the country connected to an owner             |
| `Owner`           | Represents a Pokémon owner                               |
| `Reviewer`        | Represents a user who writes reviews                     |
| `Review`          | Represents a review and rating for a Pokémon             |
| `PokemonCategory` | Many-to-many relationship between Pokémon and categories |
| `PokemonOwner`    | Many-to-many relationship between Pokémon and owners     |

---

## 📡 API Endpoints

Swagger UI is available locally after running the project:

```bash
https://localhost:{port}/swagger
```

---

### Pokémon

| Method   | Endpoint                                                             | Description                      |
| -------- | -------------------------------------------------------------------- | -------------------------------- |
| `GET`    | `/api/pokemon`                                                       | Get all Pokémon                  |
| `GET`    | `/api/pokemon/{pokeId}`                                              | Get a Pokémon by ID              |
| `GET`    | `/api/pokemon/{pokeId}/rating`                                       | Get a Pokémon rating             |
| `POST`   | `/api/pokemon?ownerId={ownerId}&categoryId={categoryId}`             | Create a new Pokémon             |
| `PUT`    | `/api/pokemon/{pokemonId}?ownerId={ownerId}&categoryId={categoryId}` | Update a Pokémon                 |
| `DELETE` | `/api/pokemon/{pokemonId}`                                           | Delete a Pokémon and its reviews |

---

### Category

| Method   | Endpoint                             | Description             |
| -------- | ------------------------------------ | ----------------------- |
| `GET`    | `/api/category`                      | Get all categories      |
| `GET`    | `/api/category/{categoryId}`         | Get a category by ID    |
| `GET`    | `/api/category/pokemon/{categoryId}` | Get Pokémon by category |
| `POST`   | `/api/category`                      | Create a new category   |
| `PUT`    | `/api/category/{categoryId}`         | Update a category       |
| `DELETE` | `/api/category/{categoryId}`         | Delete a category       |

---

### Country

| Method   | Endpoint                   | Description          |
| -------- | -------------------------- | -------------------- |
| `GET`    | `/api/country`             | Get all countries    |
| `GET`    | `/api/country/{countryId}` | Get a country by ID  |
| `GET`    | `/owners/{ownerId}`        | Get country by owner |
| `POST`   | `/api/country`             | Create a new country |
| `PUT`    | `/api/country/{countryId}` | Update a country     |
| `DELETE` | `/api/country/{countryId}` | Delete a country     |

---

### Owner

| Method   | Endpoint                           | Description          |
| -------- | ---------------------------------- | -------------------- |
| `GET`    | `/api/owner`                       | Get all owners       |
| `GET`    | `/api/owner/{ownerId}`             | Get an owner by ID   |
| `GET`    | `/api/owner/{ownerId}/pokemon`     | Get Pokémon by owner |
| `POST`   | `/api/owner?countryId={countryId}` | Create a new owner   |
| `PUT`    | `/api/owner/{ownerId}`             | Update an owner      |
| `DELETE` | `/api/owner/{ownerId}`             | Delete an owner      |

---

### Review

| Method   | Endpoint                                                    | Description               |
| -------- | ----------------------------------------------------------- | ------------------------- |
| `GET`    | `/api/review`                                               | Get all reviews           |
| `GET`    | `/api/review/{reviewId}`                                    | Get a review by ID        |
| `GET`    | `/api/review/pokemon/{pokemonId}`                           | Get reviews for a Pokémon |
| `POST`   | `/api/review?reviewerId={reviewerId}&pokemonId={pokemonId}` | Create a new review       |
| `PUT`    | `/api/review/{reviewId}`                                    | Update a review           |
| `DELETE` | `/api/review/{reviewId}`                                    | Delete a review           |

---

### Reviewer

| Method   | Endpoint                             | Description             |
| -------- | ------------------------------------ | ----------------------- |
| `GET`    | `/api/reviewer`                      | Get all reviewers       |
| `GET`    | `/api/reviewer/{reviewerId}`         | Get a reviewer by ID    |
| `GET`    | `/api/reviewer/{reviewerId}/reviews` | Get reviews by reviewer |
| `POST`   | `/api/reviewer`                      | Create a new reviewer   |
| `PUT`    | `/api/reviewer/{reviewerId}`         | Update a reviewer       |
| `DELETE` | `/api/reviewer/{reviewerId}`         | Delete a reviewer       |

---

## 🚀 Getting Started

### Prerequisites

Make sure you have the following installed:

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [SQL Server](https://www.microsoft.com/en-us/sql-server)
* [SQL Server Management Studio](https://learn.microsoft.com/en-us/ssms/download-sql-server-management-studio-ssms) or another SQL client
* Visual Studio 2022 or VS Code
* EF Core CLI tools

Install EF Core CLI tools if needed:

```bash
dotnet tool install --global dotnet-ef
```

---

## 📥 Clone the Repository

```bash
git clone https://github.com/beqa21beraia/PokemonReviewApp.git
cd PokemonReviewApp
```

Move into the main API project:

```bash
cd PokemonReviewApp
```

---

## ⚙️ Configuration

Update the connection string in `appsettings.json`.

Example local SQL Server configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=PokemonReview;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Example SQL Server Express configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_PC_NAME\\SQLEXPRESS;Database=PokemonReview;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> ⚠️ Do not commit real passwords, production connection strings, or private database credentials to source control.

---

## 🗄 Database Setup

This project uses **Entity Framework Core migrations** to create and update the SQL Server database schema.

Apply the existing migrations:

```bash
dotnet ef database update
```

To create a new migration after changing models:

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

---

## 🌱 Seed the Database

The project includes a `Seed.cs` file for inserting initial sample data.

To run the application with seed data:

```bash
dotnet run -- seeddata
```

After seeding, you can run the app normally:

```bash
dotnet run
```

---

## ▶️ Run the Application

From the `PokemonReviewApp/PokemonReviewApp` project folder, run:

```bash
dotnet restore
dotnet build
dotnet run
```

Then open Swagger UI in your browser:

```bash
https://localhost:{port}/swagger
```

The exact port will be shown in the terminal when the app starts.

---

## 🧪 Example Requests

### Create a Category

```http
POST /api/category
Content-Type: application/json

{
  "id": 0,
  "name": "Electric"
}
```

### Create a Country

```http
POST /api/country
Content-Type: application/json

{
  "id": 0,
  "name": "Japan"
}
```

### Create an Owner

```http
POST /api/owner?countryId=1
Content-Type: application/json

{
  "id": 0,
  "firstName": "Ash",
  "lastName": "Ketchum",
  "gym": "Pallet Town Gym"
}
```

### Create a Pokémon

```http
POST /api/pokemon?ownerId=1&categoryId=1
Content-Type: application/json

{
  "id": 0,
  "name": "Pikachu",
  "birthDate": "2020-01-01T00:00:00"
}
```

### Create a Reviewer

```http
POST /api/reviewer
Content-Type: application/json

{
  "id": 0,
  "firstName": "Brock",
  "lastName": "Harrison"
}
```

### Create a Review

```http
POST /api/review?reviewerId=1&pokemonId=1
Content-Type: application/json

{
  "id": 0,
  "title": "Great Pokémon",
  "text": "Pikachu is fast, loyal, and powerful.",
  "rating": 5
}
```

---

## 🪵 Logging

The project uses **Serilog** for structured logging.

Logging is configured for:

| Sink    | Description                              |
| ------- | ---------------------------------------- |
| Console | Shows logs while running the API locally |
| File    | Stores logs in the `Logs/` folder        |

Log files are generated with daily rolling behavior:

```bash
Logs/log-.txt
```

The API also uses request logging middleware, so HTTP requests are automatically logged.

Common log levels used in the project:

| Level         | Usage                                               |
| ------------- | --------------------------------------------------- |
| `Information` | Successful create/delete operations                 |
| `Warning`     | Not found resources, duplicates, invalid operations |
| `Error`       | Database save/update/delete failures                |

---

## 🧠 What This Project Demonstrates

This project is a strong practical example of backend development with ASP.NET Core. It demonstrates:

* Building REST APIs with controllers
* Working with SQL Server through Entity Framework Core
* Creating and applying database migrations
* Using repository interfaces and implementations
* Mapping DTOs and models with AutoMapper
* Handling many-to-many relationships
* Writing asynchronous service/repository methods
* Returning proper HTTP responses
* Adding structured logs for debugging and monitoring
* Testing APIs through Swagger UI

---

## 🔮 Possible Future Improvements

* Add JWT authentication and authorization
* Add role-based access control
* Add pagination, filtering, and sorting
* Add FluentValidation for request validation
* Add unit tests and integration tests
* Add global exception handling middleware
* Add API versioning
* Add Docker support
* Add CI/CD pipeline with GitHub Actions or Azure Pipelines
* Deploy API and database to Azure

---

## 🤝 Contributing

Contributions are welcome.

To contribute:

1. Fork the repository
2. Create a new branch:

```bash
git checkout -b feature/my-feature
```

3. Commit your changes:

```bash
git commit -m "Add my feature"
```

4. Push your branch:

```bash
git push origin feature/my-feature
```

5. Open a Pull Request

---

## 📄 License

This project is intended for educational purposes.
Feel free to fork it, study it, and use it as a reference for your own ASP.NET Core Web API projects.

---

<div align="center">

Built with ❤️ using ASP.NET Core 8, Entity Framework Core, SQL Server, and C#.

</div>
