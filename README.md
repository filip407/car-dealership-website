# Car Dealership Web Application
 
A web application for managing a car dealership — built with ASP.NET Core (.NET 9), Entity Framework Core, PostgreSQL, and ASP.NET Core Identity. The app covers car inventory, sales tracking, test drive scheduling, wishlists, and admin statistics.
 
---
 
## Screenshots
 
### Home Page
<img width="1111" height="581" alt="homepage" src="https://github.com/user-attachments/assets/c1107c08-07dc-47a5-9b07-2eca0f144107" />


### Car Listing
<img width="1118" height="576" alt="listings" src="https://github.com/user-attachments/assets/a677a4d5-9c32-42bf-a1c3-c26329553a36" />

 
### Car Details
<img width="1087" height="422" alt="details" src="https://github.com/user-attachments/assets/2da78067-d162-472e-86a4-19c1a744f3ff" />


### Car Details (Customer)
<img width="1113" height="563" alt="details-customer" src="https://github.com/user-attachments/assets/8f13508e-232d-43e6-b40f-f9990b6eea50" />


### Create / Edit Car (Admin)
<img width="1119" height="578" alt="create-car" src="https://github.com/user-attachments/assets/0c0b94c0-e24b-4c56-a687-ea652b6a2f3c" />


### Sell Car (Admin)
<img width="1114" height="575" alt="sell" src="https://github.com/user-attachments/assets/a0448bae-0cb8-4873-9c9e-974efc842d50" />

 
### Test Drive Booking
<img width="1115" height="568" alt="booking" src="https://github.com/user-attachments/assets/a2ea9a8c-8cdf-49c6-96df-c32617fb8474" />

 
### Test Drive Calendar (Admin)
<img width="1118" height="537" alt="test-drive-calendar" src="https://github.com/user-attachments/assets/49bfd212-0100-48fb-a3ce-8768e7a12815" />


### Wishlist
<img width="1112" height="568" alt="wishlist" src="https://github.com/user-attachments/assets/14044794-52f5-4290-8122-7e92eee39039" />

 
### Statistics (Admin)
<img width="1117" height="559" alt="statistics" src="https://github.com/user-attachments/assets/a170363b-2ed4-42bd-b7dc-598aa2a248c9" />

 
### Login / Register
<img width="1112" height="438" alt="register" src="https://github.com/user-attachments/assets/57fd0372-9350-4d75-8f91-a93c3ab7147e" />
<img width="1120" height="572" alt="client-account" src="https://github.com/user-attachments/assets/fb6cb127-af25-42f4-880b-6e7767da1831" />


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
