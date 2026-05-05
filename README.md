# PasteleriaApp API 🧁 (Backend Refactored)

Welcome to the **PasteleriaApp** API! This is a robust backend developed by **Feith Noir** and optimized for efficient management of a modern bakery.

## ✨ Project Purpose

This backend provides a solid, scalable, and professional foundation to manage:

*   **Ingredients:** Centralized database of raw materials with costs and suppliers.
*   **Inventory:** Real-time stock control with low-level alerts.
*   **Recipes:** Detailed recipe definition with automatic cost calculation.
*   **Documentation:** Registration and management of important business documents.
*   **Security:** JWT authentication system and roles (`Admin`, `User`, `Visitor`).

## 🛠️ Technology Stack (Updated)

*   **Backend:**
    *   **Framework:** .NET 8.0 (C#) - ASP.NET Core API.
    *   **Database:** SQLite (for agile development) - `pasteleriapp.db`.
    *   **Data Access:** Entity Framework Core with Repository Pattern.
    *   **Object Mapping:** AutoMapper for clean communication between layers.
    *   **Authentication:** ASP.NET Core Identity with JWT Bearer Tokens.
    *   **Documentation:** Swagger (OpenAPI) with XML comments support.

## 🏗️ Architecture and Best Practices

*   **Clean Architecture:** Clear separation between `Data`, `Business`, `Shared`, and `API`.
*   **Repository Pattern:** Total abstraction of data access, facilitating unit testing.
*   **Optimized DTOs:** Use of Data Transfer Objects to avoid redundancies and entity leaks to the client.
*   **Dependency Injection:** Native management of services and repositories.
*   **Standardized Responses:** All API responses follow the `ApiResponse<T>` format, guaranteeing consistency for the frontend.

## 🔮 Future Improvements and Best Practices

To take this project to the next level of robustness and professionalism, the following implementations are considered:

*   **Refresh Tokens:** Implementation of a token rotation system to improve security and user experience without requiring constant logins.
*   **Advanced Security with Bearer Tokens:** Granular restriction of sensitive endpoints through authorization policies based on roles and claims.
*   **Validation with FluentValidation:** Decouple validation logic from DTOs for cleaner and more maintainable code.
*   **Centralized Logging:** Integration with **Serilog** to persist logs in files, databases, or cloud services (Seq, Azure Application Insights).
*   **Automated Testing:** Unit test coverage with **xUnit** and integration tests to ensure system stability.
*   **Rate Limiting:** Protection against brute force attacks and API abuse by limiting the number of requests per client.
*   **Data Caching:** Implementation of **Redis** or in-memory cache for high-demand endpoints such as the ingredient or recipe list.
*   **CI/CD Pipelines:** Deployment automation through GitHub Actions to ensure every change passes tests and is deployed safely.

## 💾 Data Model (Main Entities)

*   `User` (Managed by ASP.NET Identity)
*   `Ingredient` (Raw materials)
*   `InventoryItem` (Ingredient stock)
*   `Recipe` (Product technical sheet)
*   `RecipeIngredient` (Ingredient-recipe relationship)
*   `Document` (Documentary record)

## 🚀 Getting Started with the Backend

1.  **.NET 8.0 SDK:** Make sure you have the latest version of .NET installed.
2.  **Configuration:** Check `appsettings.json` for JWT keys and the SQLite database path.
3.  **Database:** The project already includes a preconfigured SQLite database. If you want to reset it, use: `dotnet ef database update`.
4.  **Run:** `dotnet run` inside the `Pasteleria.Api` folder.
5.  **Swagger:** Access `http://localhost:5000/swagger` to see the interactive documentation.

---

## 🧑‍💻 Developed by: **Feith Noir**
