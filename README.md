# YeniNetCore – EF Core 8 Learning Project

**YeniNetCore** is a simple .NET 8 application created as a learning exercise for **Entity Framework Core 8**. The main purpose of this project is to practice using .NET 8 and EF Core 8 in a basic application rather than to deliver a production‑grade solution. It demonstrates fundamental EF Core operations in a minimal setup, serving as a hands‑on learning implementation.

## Prerequisites

* **.NET 8 SDK** – Install .NET 8 (EF Core 8 targets .NET 8). You can use the .NET CLI or an IDE like Visual Studio 2022 (v17.4 or later).
* **Database** – Microsoft SQL Server (e.g., SQL Server Express or LocalDB). By default, the connection string points to a local instance; adjust it to suit your environment.
* **Entity Framework Core 8** – Delivered via NuGet; the project already references:

  * `Microsoft.EntityFrameworkCore`
  * `Microsoft.EntityFrameworkCore.SqlServer`
  * `Microsoft.EntityFrameworkCore.Tools` (for migrations)

## Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/burhanmorningstar/YeniNetCore.git
   cd YeniNetCore
   ```

2. **Configure the database**

   Edit the connection string in `appsettings.json` or in your `DbContext` configuration to match your SQL Server instance.

3. **Restore dependencies**

   ```bash
   dotnet restore
   ```

4. **Apply migrations**

   ```bash
   dotnet ef database update
   ```

5. **Build & run**

   ```bash
   dotnet run
   ```

## Usage

When the application starts, it executes basic CRUD operations that demonstrate EF Core 8 usage. Output appears in the console (or via minimal API endpoints if provided). Feel free to extend the code—add entities, experiment with LINQ queries, enable logging to inspect generated SQL, etc.

## Dependencies Overview

| Dependency                              | Purpose                    |
| --------------------------------------- | -------------------------- |
| .NET 8 SDK                              | Target framework & runtime |
| Microsoft.EntityFrameworkCore           | Core EF runtime            |
| Microsoft.EntityFrameworkCore.SqlServer | SQL Server provider        |
| Microsoft.EntityFrameworkCore.Tools     | CLI tooling & migrations   |
| SQL Server (Express/LocalDB)            | Database                   |

---

This repository is purely for educational purposes and is **not** intended for production use.
