# Application Development Summary

## Purpose
This application is designed for **educational purposes**, focusing on demonstrating best practices in software development. It serves as a practical example for building a complete application from scratch using the latest technologies and frameworks.

## Technologies in Use
- **Entity Framework**
- **XUnit**
- **ASP.NET Core MVC**
- Other related technologies

## Completed Components
- **AutoLot.Dal**: Data Access Layer
- **AutoLot.Dal.Tests**: Unit Testing for DAL
- **AutoLot.Models**: Data Models
- **AutoLot.Api**: RESTful API
- **AutoLot.MVC**: Frontend (Blazor)

### Included Features
- **Migrations**: Managing schema changes with database migrations.
- **Exceptions**: Custom exception handling for robust error management.
- **Initialization**: Database initialization and seeding for setup.
- **Repos**: Repository pattern implementation for organized data access.
- **Entities**: Domain entities representing the database structure.
- **Integration Tests**: Tests covering the integration between components.
- **ViewModels**: Data transfer objects for communication between controller and views.
- **SeliLog**: Enhanced logging using SeliLog for better diagnostics.
- **ASP.NET Core MVC/Web API**: Development of web and API interfaces.
- **Controllers**: MVC controllers handling requests and business logic.
- **Filters**: Custom filters for request and response management.
- **Models**: Development and refinement of data models.
- **ViewComponents**: Reusable view logic components.
- **Views**: Creation of user interfaces for the MVC application.
- **AutoLot.Services**: Service layer for business logic implementation.
- **ApiWrapper**: Wrapping external API calls within the application.
- **Logging**: Advanced logging mechanisms for detailed diagnostics.
- **TagHelpers**: Custom tag helpers for improved view rendering (Ready only for Car).
- **BackUp**: Database Back-Up.
- **Docker**: .

## Remaining Tasks
- [ ] ...

# How to Run the Application

Follow these steps to set up and run the application:

1. **Clone the Repository**  
   Use the following command to clone the repository to your local machine:
   ```bash
   git clone https://github.com/Vlad6422/AutoLot.git
   ```

2. **Open in Visual Studio**  
   Open the solution in **Visual Studio**. When the solution loads, Visual Studio will automatically download all the required packages.

3. **Update Connection Strings**  
   Update the database connection strings in the **AutoLot.Dal**, **AutoLot.Api**, and **AutoLot.MVC** projects:
   - Locate the connection string section in the `appsettings.json` file in each project.
   - In Dal change Factory class.
   - Modify the connection string to point to your local or remote database server.

4. **Apply Database Migrations** (in Dev Console & AutoLot.Dal Project)  
   Open the **Developer Console** and navigate to the **AutoLot.Dal** project directory. Then run the following command to apply the database migrations:
   ```bash
   dotnet ef database update
   ```

   Alternatively, if a backup of the database is available, you can restore it to your server.

5. **Configure Startup**  
   Ensure that both the **API** and **MVC** projects are set to run during startup:
   - Right-click the solution in **Visual Studio**.
   - Select **Properties**.
   - Under **Startup Project**, select **Multiple startup projects** and set both the **API** and **MVC** projects to start.

6. **Run the Application**  
   Hit **F5** in Visual Studio to build and run the application. The API and the MVC front-end should now be accessible.
## Screenshots

**API:**
- ![Cars](docs/img/api/Cars.png)
- ![Credit_Customers](docs/img/api/Credit_Customers.png)
- ![Makes_Orders](docs/img/api/Makes_Orders.png)

**Frontend:**
- ![HomePage](docs/img/mvc/HomePage.png)
- ![Inventory](docs/img/mvc/Inventory.png)
- ![CreateCar](docs/img/mvc/CreateCar.png)
- ![Customers](docs/img/mvc/Customers.png)
- ![Orders](docs/img/mvc/Orders.png)
