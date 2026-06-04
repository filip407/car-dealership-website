# CarDealership

A web application for managing a car dealership, built with ASP.NET Core (.NET 9), Entity Framework Core, PostgreSQL, and ASP.NET Core Identity. The app covers car inventory management, sales tracking, test drive scheduling, wishlists, and an admin statistics dashboard.

**Student:** Dumitriu Filip — Grupa 363

---

## Screenshots

### Home Page
<!-- Screenshot: Home page -->

### Car Listing
<!-- Screenshot: /Cars/Index -->

### Car Details
<!-- Screenshot: /Cars/Details/{id} -->

### Create / Edit Car (Admin)
<!-- Screenshot: Create or Edit form with brand dropdown, features checkboxes, and image upload -->

### Sell Car (Admin)
<!-- Screenshot: Sell form with negotiated price input -->

### Test Drive Booking
<!-- Screenshot: /TestDrives/Book/{carId} -->

### Test Drive Calendar (Admin)
<!-- Screenshot: /TestDrives/Calendar -->

### Wishlist
<!-- Screenshot: /Wishlist -->

### Statistics (Admin)
<!-- Screenshot: /Statistics — revenue, cars sold, per-agent breakdown -->

### Login / Register
<!-- Screenshot: Auth forms -->

---

## Tech Stack

- **Backend:** ASP.NET Core MVC + Web API, .NET 9
- **ORM:** Entity Framework Core 9 with Npgsql (PostgreSQL)
- **Auth:** ASP.NET Core Identity — cookie-based sessions, roles `Admin` / `Customer`
- **Architecture:** Service Layer + Repository Pattern + Unit of Work + DTOs
- **Frontend:** Razor Views (server-rendered) + Vue.js component on the Features page consuming `/api/features`
- **Database:** PostgreSQL

---

## Project Structure

```
CarDealership/
├── Controllers/
│   ├── Api/                  # REST API controllers
│   │   └── FeaturesApiController.cs
│   ├── AuthController.cs
│   ├── BrandsController.cs
│   ├── CarsController.cs
│   ├── FeaturesController.cs
│   ├── HomeController.cs
│   ├── SettingsController.cs
│   ├── StatisticsController.cs
│   ├── TestDrivesController.cs
│   └── WishlistController.cs
├── Data/
│   ├── AppDbContext.cs
│   └── DbSeeder.cs           # Seeds roles and default admin account on startup
├── DTOs/                     # Request/response shapes for the API layer
├── Mappings/                 # Extension methods: entity <-> DTO / ViewModel
├── Middleware/
│   └── ExceptionMiddleware.cs
├── Migrations/               # EF Core migrations
├── Models/                   # Database entities
├── Repositories/             # Generic repository + Unit of Work
├── Services/                 # Business logic
├── ViewModels/               # View-specific models with validation attributes
└── Views/                    # Razor views, one folder per feature
```

---

## Data Model

| Entity | Notes |
|---|---|
| `ApplicationUser` | Extends `IdentityUser`. Agents are users with the `Admin` role. |
| `Brand` | One-to-Many with `Car` |
| `Car` | Belongs to a `Brand` and an agent (`ApplicationUser`). Has an optional image. |
| `Feature` | Many-to-Many with `Car` (e.g. leather seats, sunroof, parking sensors) |
| `Sale` | Records a completed sale — links a `Car`, an agent, a price, and a date |
| `TestDrive` | A scheduled test drive request, with confirmation status and optional notes |
| `WishlistItem` | Links a `Customer` to a `Car` they've saved |

---

## How It Works

### Car Inventory

Cars are listed with brand, model, year, price, features, and a photo. Each car can be in one of two states: available or sold. Admins can create and edit cars, assign features, upload an image, and mark a car as sold at a negotiated price — which records a `Sale` entry and automatically removes the car from all wishlists and pending test drives.

### Test Drives

Customers can request a test drive for any available car, picking a date and leaving optional notes. Admins see all requests in a calendar view and can confirm or cancel them. Each user also has a personal view of their own upcoming and past test drives.

### Wishlist

Customers can save cars they're interested in. The wishlist is per-user and updates in real time via a small fetch call on the Details page.

### Features (Vue.js + REST API)

The Features page uses a Vue.js component that communicates with `/api/features` directly. Admins can add and delete features without page reloads. The component handles loading and error states.

### Statistics

Admins have access to a dashboard showing total revenue, number of cars sold, a per-agent breakdown (cars sold and revenue), and a table of the 10 most recent sales.

---

## API

`FeaturesApiController` exposes a RESTful endpoint at `/api/features`, used by the Vue.js component on the Features page. Entities are never exposed directly — responses use DTOs.

| Method | Endpoint | Auth | Response |
|---|---|---|---|
| GET | `/api/features` | — | 200 |
| GET | `/api/features/{id}` | — | 200 / 404 |
| POST | `/api/features` | Admin | 201 + Location / 400 / 401 / 403 |
| PUT | `/api/features/{id}` | Admin | 204 / 400 / 401 / 403 / 404 |
| DELETE | `/api/features/{id}` | Admin | 204 / 401 / 403 / 404 |

Unhandled exceptions are caught by `ExceptionMiddleware` and returned as JSON with a `500` status code. The service layer logs operations via the built-in `ILogger<T>`.

---

## Authentication & Authorization

Users register with a name, email, and password and are assigned the `Customer` role. Authentication uses ASP.NET Core Identity with cookie-based sessions. Pages that require login redirect unauthenticated users to `/Auth/Login`. Admin-only pages (statistics, car management, settings, test drive calendar) return `403` for non-admin users.

An admin account is seeded automatically on first startup:

| Field | Value |
|---|---|
| Email | `admin@cardealership.com` |
| Password | `Admin123!` |

---

## Setup

Requires .NET 9 SDK and a running PostgreSQL instance. Set the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=cardealership_db;Username=postgres;Password=YOUR_PASSWORD"
}
```

Then run:

```bash
dotnet run
```

Migrations and the admin seed run automatically on startup.
