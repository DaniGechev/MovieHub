# 🎬 MovieHub

A movie review catalog web application built with **ASP.NET Core MVC**. Users can browse a catalog of films, read and write reviews with ratings, and build a personal list of favorite movies. Administrators manage the catalog (movies and genres).

This project was developed as a final project for the SoftUni *ASP.NET Core MVC* course.

---

## ✨ Features

- **Movie catalog** — browse all movies, with searching (by title or director), sorting (newest, oldest, alphabetical, highest rated) and pagination.
- **Movie details** — full information about each film, its genre, average rating and all reviews.
- **Reviews** — logged-in users can write a review with a rating from 1 to 10. Adding and deleting reviews works without a page reload (AJAX).
- **Favorites / Watchlist** — logged-in users can add or remove movies from their personal favorites list (AJAX toggle).
- **Authentication & roles** — registration and login via ASP.NET Core Identity, with an **Administrator** role.
- **Admin management** — administrators can create, edit and delete movies and genres.
- **Data validation** and **security** — model validation, anti-forgery tokens (CSRF), output encoding (XSS) and role-based authorization.

---

## 🛠️ Technologies

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core (Code First) + SQL Server (LocalDB)
- ASP.NET Core Identity
- Bootstrap 5
- NUnit, Moq and EF Core InMemory (unit tests)

---

## 🏗️ Project structure

The solution is split into four projects for a clean separation of concerns:

| Project | Responsibility |
|---|---|
| **MovieHub.Data** | Entity models, `ApplicationDbContext`, EF Core migrations and seed data |
| **MovieHub.Services** | Business logic — service interfaces and implementations + service models (DTOs) |
| **MovieHub.Web** | Controllers, Razor views, view models, Identity and AJAX |
| **MovieHub.Tests** | Unit tests for the services (EF InMemory) and a controller (Moq) |

### Data model

- **Movie** — belongs to one **Genre**, has many **Reviews**
- **Genre** — has many **Movies**
- **Review** — belongs to a **Movie** and an **ApplicationUser**
- **Favorite** — many-to-many link between **ApplicationUser** and **Movie**
- **ApplicationUser** — extends `IdentityUser` with custom properties (`NickName`, `ProfilePictureUrl`)

---

## 🚀 Getting started

### Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022 (or any IDE) with **SQL Server LocalDB** (included with the "ASP.NET and web development" workload)

### Run the project

1. Clone the repository:
   ```
   git clone https://github.com/DaniGechev/MovieHub.git
   ```
2. Open `MovieHub.sln` in Visual Studio.
3. Make sure **MovieHub.Web** is set as the startup project.
4. Press **F5**.

On first launch the application automatically:
- creates the `MovieHubDb` database,
- applies the EF Core migrations,
- seeds the genres and sample movies,
- creates the administrator account and role.

No manual database setup is required.

### Connection string

The database connection is configured in `MovieHub.Web/appsettings.json`. By default it uses LocalDB:

```
Server=(localdb)\MSSQLLocalDB;Database=MovieHubDb;Trusted_Connection=True;...
```

---

## 👤 Default accounts

A seeded administrator account is available out of the box:

| Role | Email | Password |
|---|---|---|
| Administrator | `admin@moviehub.com` | `Admin123!` |

Regular users can be created through the **Register** page. (Passwords require at least 6 characters, including an uppercase letter, a lowercase letter and a digit.)

---

## 🧪 Running the tests

From Visual Studio: **Test → Run All Tests**, or from the command line:

```
dotnet test
```

The tests cover the business logic in the service layer (using an in-memory database) and the `MoviesController` (using Moq to mock the services).

---

## 📄 License

This project was created for educational purposes as part of the SoftUni ASP.NET Core MVC course.
