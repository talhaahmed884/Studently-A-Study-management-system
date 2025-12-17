# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Studently is a study management system built with ASP.NET Core Web API targeting .NET 10.0. The project is currently in its initial scaffolding phase with template controller code.

## Development Commands

### Build and Run
- Build the project: `dotnet build`
- Run the application: `dotnet run`
- Run with watch (auto-reload): `dotnet watch run`
- Clean build artifacts: `dotnet clean`

### Testing
- Run all tests: `dotnet test`
- Run tests with coverage: `dotnet test /p:CollectCoverage=true`
- Run specific test: `dotnet test --filter FullyQualifiedName~TestClassName.TestMethodName`

### Package Management
- Restore dependencies: `dotnet restore`
- Add package: `dotnet add package <PackageName>`
- Remove package: `dotnet remove package <PackageName>`

### Development Tools
- Format code: `dotnet format`
- List outdated packages: `dotnet list package --outdated`

## Architecture

### Project Structure
- **Controllers/**: API controllers implementing RESTful endpoints
- **Program.cs**: Application entry point and service configuration
- **appsettings.json**: Application configuration for all environments
- **appsettings.Development.json**: Development-specific overrides

### Technology Stack
- **Framework**: .NET 10.0 (Microsoft.NET.Sdk.Web)
- **API Documentation**: OpenAPI/Swagger (enabled in development via `app.MapOpenApi()`)
- **Features**:
  - Nullable reference types enabled
  - Implicit usings enabled
  - HTTPS redirection
  - Authorization middleware configured

### Application Startup
The application uses minimal hosting model with `WebApplication.CreateBuilder()`. Services are registered in Program.cs:
- Controllers via `AddControllers()`
- OpenAPI documentation via `AddOpenApi()`

Middleware pipeline (in order):
1. OpenAPI endpoint (development only)
2. HTTPS redirection
3. Authorization
4. Controller routing

### API Conventions
- Controllers inherit from `ControllerBase` and use `[ApiController]` attribute
- Route template follows `[Route("[controller]")]` pattern
- HTTP methods use named routes (e.g., `Name = "GetWeatherForecast"`)
- Controllers are placed in the `Studently.Controllers` namespace

### Development Environment
The application runs in development mode based on `ASPNETCORE_ENVIRONMENT`. In development:
- OpenAPI endpoint is mapped at `/openapi/v1.json`
- Detailed logging is available
- Developer exception pages should be enabled (standard for ASP.NET Core)

### Configuration
Configuration sources are loaded automatically in this order (later overrides earlier):
1. appsettings.json
2. appsettings.{Environment}.json
3. Environment variables
4. Command-line arguments
