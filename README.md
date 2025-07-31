# ASP.NET Core Full-Stack Solution

This repository contains a comprehensive full-stack web solution built using modern ASP.NET Core technologies. The system consists of a Razor Pages/MVC web application, RESTful API, and background task processing components.

## Solution Components

### 1. Web Application (WebApplication1)
- ASP.NET Core Razor Pages/MVC architecture
- JWT-based authentication and session management
- User management and admin panel
- Product listing and management
- Log viewing and system monitoring
- Responsive interface with Bootstrap and jQuery

### 2. WebAPI
- .NET 9 Web API
- JWT authentication and authorization
- Entity Framework Core with SQL Server integration
- Redis cache management
- Rate limiting and API security
- Swagger/OpenAPI documentation
- Centralized error handling

### 3. Background Processing Service (HangfireConsoleApp)
- Scheduled tasks with Hangfire
- Standalone console application
- Background worker for asynchronous operations

## ASP.NET Core Technologies Used

- **Authentication & Authorization**: JWT token-based authentication, role-based authorization
- **Data Access**: Entity Framework Core, SQL Server
- **API Development**: RESTful API design, Swagger/OpenAPI
- **UI Development**: Razor Pages, MVC, Bootstrap
- **Performance Optimization**: Redis caching, rate limiting
- **Background Processing**: Job scheduling with Hangfire
- **Logging & Monitoring**: NLog integration, custom log API
- **Security**: JWT validation, password hashing (BCrypt)
- **Deployment & Configuration**: Multi-environment configurations, connection resilience

## Setup

### Requirements
- .NET 9 SDK
- SQL Server
- Redis Server
- Visual Studio 2022 (recommended)

### Steps
1. Clone the repository
2. Configure the `appsettings.json` files in each project for your environment:
   - SQL Server connection string
   - Redis connection information
   - JWT key and configuration
3. Create the SQL Server database:
dotnet ef database update --project WebAPI

4. Ensure Redis server is running
5. Start the projects in the following order:
- WebAPI
- HangfireConsoleApp (optional)
- WebApplication1

## Project Structure

- **WebApplication1/**: Razor Pages/MVC web application
- **WebAPI/**: RESTful API services
- **HangfireConsoleApp/**: Background processing service
- **Shared/**: Shared models and helper classes (if any)

## Development Notes

- WebAPI is configured to run over HTTPS
- Swagger interface is accessible at `/swagger`
- JWT token is required for API communication
- Session information is stored in the session
- NLog configuration is available for debugging and testing

## License

This project is licensed under the [MIT License](LICENSE).

## Contributing

To contribute, please open an issue or submit a pull request.
