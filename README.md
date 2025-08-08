YeniNetCore – EF Core 8 Learning Project
YeniNetCore is a simple .NET 8 application created as a learning exercise for Entity Framework Core 8. The main purpose of this project is to practice using .NET 8 and EF Core 8 in a basic application, rather than to deliver a production-grade solution. It demonstrates fundamental EF Core operations in a minimal setup, serving as a hands-on learning implementation.
Prerequisites
•	.NET 8 SDK – Install .NET 8 (EF Core 8 requires .NET 8 as the target framework[1]). You can use the .NET CLI or an IDE like Visual Studio 2022 (v17.4 or later) that supports .NET 8.
•	Database – The project uses Microsoft SQL Server for data storage. Ensure you have access to a SQL Server instance (for example, SQL Server Express or LocalDB on Windows). By default, the connection string may point to a local SQL Server (e.g. using LocalDB)[2] – you can adjust this if needed for your environment.
•	Entity Framework Core 8 – The required EF Core 8 NuGet packages (e.g. Microsoft.EntityFrameworkCore and the SQL Server provider Microsoft.EntityFrameworkCore.SqlServer) are already specified in the project. No manual installation is needed if you build the project, but note that EF Core is delivered via NuGet packages (one must install the provider package for the chosen database)[3][4]. The project also references the appropriate EF Core design/tools packages for migrations.
Installation and Setup
Follow these steps to set up and run the project locally:
1.	Clone the Repository: Clone this GitHub repository to your local machine using Git:
git clone https://github.com/burhanmorningstar/YeniNetCore.git
Then navigate into the repository directory (YeniNetCore).
1.	Configure the Database (if necessary): Open the project’s configuration (for example, the appsettings.json or the DbContext configuration in code) and check the connection string. By default it may use a LocalDB SQL Server instance (with a connection string like Server=(localdb)\mssqllocaldb;Database=...;Trusted_Connection=True;)[2]. If you are not on Windows or don’t have LocalDB, update the connection string to point to your SQL Server or adjust the server name, credentials, and database name as needed.
2.	Restore Dependencies: The project’s NuGet dependencies (EF Core packages, etc.) should restore automatically on build. If you’re using the .NET CLI, you can explicitly restore by running dotnet restore. (In Visual Studio, opening the solution will trigger a restore as well.)
3.	Apply Migrations (Database Setup): If the project includes Entity Framework Core migrations for setting up the database schema, run them before launching the application. Using the .NET CLI, you can run the following command in the project directory to create the database and tables:
dotnet ef database update
This will apply the latest migration and set up the database schema (assuming migrations have been defined in the project)[5]. Make sure the SQL Server specified in your connection string is running and accessible.
1.	Build and Run the Application: You can now build and run the project:
2.	Using .NET CLI: In the repository directory, execute dotnet run to compile and launch the application[6]. This will build the project (restoring packages if needed) and then start the application.
3.	Using Visual Studio: Open the YeniNetCore project (or solution) in Visual Studio. Build the solution (e.g., Build > Build Solution), then run the project (e.g., press F5 or select Debug > Start Without Debugging to run without the debugger attached). This will launch the application in the configured environment.
Usage
Once the application is running, it will execute the programmed EF Core operations. Since this is a learning-oriented project, it likely performs basic CRUD actions or simple demonstrations of EF Core functionality (such as adding an entity, querying data, updating, and deleting records) in a console output or via a minimal web/API interface. There is no complex user interface or extensive features – the focus is on the EF Core data access logic.
•	If it’s a console application, you should see console output indicating the EF Core actions (e.g. inserting sample data, querying, etc.).
•	If it’s a web API or similar, the README or project comments will typically instruct how to interact with the API (for example, endpoints to test). In this case, since no specific web UI is mentioned, assume it’s a console app or simple logic that runs on start.
Note: This project is for educational purposes. The code is structured in a straightforward manner without advanced architectural patterns, since the goal is to understand and experiment with EF Core 8 in a .NET 8 context. Feel free to modify the code or extend it as you learn. For example, you can add new entities, experiment with LINQ queries, or enable logging to see the generated SQL. The emphasis is on learning how EF Core 8 interacts with a SQL database in a .NET application.
Dependencies Summary
•	.NET 8 Runtime/SDK: Required to build and run the project (EF Core 8 targets .NET 8)[1].
•	Entity Framework Core 8: Used as the ORM for database access. Key packages include:
•	Microsoft.EntityFrameworkCore (core EF runtime)
•	Microsoft.EntityFrameworkCore.SqlServer (EF Core provider for SQL Server) – allows the app to use SQL Server as the database[3][4].
•	(Optionally Microsoft.EntityFrameworkCore.Tools / Design for migrations support – already referenced in the project for development tasks.)
•	SQL Server: The project is configured for SQL Server. You can use SQL Server Express or LocalDB on a development machine, or adjust the connection to any SQL Server instance. Ensure the database server is available and the connection string in the project is correct for your setup.
________________________________________
By following the above instructions, you should be able to build and run YeniNetCore on your local machine and use it as a reference or sandbox for learning Entity Framework Core 8. Enjoy experimenting with the project, and happy coding!
________________________________________
[1] Entity Framework Core 8 (EF8) is available today - .NET Blog
https://devblogs.microsoft.com/dotnet/announcing-ef8/
[2] Database Providers - EF Core | Microsoft Learn
https://learn.microsoft.com/en-us/ef/core/providers/
[3] [4] Installing Entity Framework Core - EF Core | Microsoft Learn
https://learn.microsoft.com/en-us/ef/core/get-started/overview/install
[5] [6] Getting Started - EF Core | Microsoft Learn
https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app
