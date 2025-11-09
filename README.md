# ClientlyMan

ClientlyMan is a sample insurance broker CRM backend built on **.NET 9** with **.NET Aspire**. It demonstrates how to apply Clean Architecture, SOLID principles, and modern .NET platform capabilities to build a modular, testable solution orchestrated through Aspire.

## Solution structure

```
ClientlyMan.sln
├─ src/
│  ├─ ClientlyMan.AppHost/           # .NET Aspire orchestration host
│  ├─ ClientlyMan.Api/               # ASP.NET Core Web API (REST + Swagger)
│  ├─ ClientlyMan.Application/       # Application services, DTOs, validators
│  ├─ ClientlyMan.Domain/            # Domain entities and enums
│  └─ ClientlyMan.Infrastructure/    # EF Core, repositories, migrations
└─ tests/
   ├─ ClientlyMan.Tests.Unit/        # Unit tests (services & validators)
   └─ ClientlyMan.Tests.Integration/ # API-level integration tests
```

### Layered responsibilities

- **Domain** – pure domain model: entities, enums, and domain-specific helpers.
- **Application** – use cases, service interfaces/implementations, DTOs, FluentValidation validators, and mapping helpers. Depends only on Domain.
- **Infrastructure** – database access (EF Core, PostgreSQL), repository implementations, migrations, and dependency injection helpers. Depends on Application and Domain.
- **API** – thin controllers exposing REST endpoints. Uses Application services via dependency injection and configures Swagger, validation, and HTTP pipeline.
- **AppHost** – Aspire orchestration project that wires the API to a PostgreSQL resource and handles local developer experience.
- **Tests** – unit and integration tests targeting the application and API layers respectively.

## Prerequisites

- .NET 9 SDK (preview) with Aspire workloads installed
- Docker (for Aspire-managed PostgreSQL container)

## Running with .NET Aspire

1. Navigate to the solution root.
2. Execute the AppHost project:
   ```bash
   dotnet run --project src/ClientlyMan.AppHost/ClientlyMan.AppHost.csproj
   ```
3. Aspire will spin up the orchestrator, launch PostgreSQL, apply configuration, and start the API project. The dashboard will show service health and exposed endpoints.
4. Open the Swagger UI (link provided by Aspire or default `http://localhost:xxxx/swagger`) to explore the API.

## Database migrations

Migrations are included under `src/ClientlyMan.Infrastructure/src/Migrations`.

To add a new migration (requires EF tooling and PostgreSQL connection available):
```bash
dotnet ef migrations add <MigrationName> \
  --project src/ClientlyMan.Infrastructure/ClientlyMan.Infrastructure.csproj \
  --startup-project src/ClientlyMan.Api/ClientlyMan.Api.csproj
```

To apply migrations manually:
```bash
dotnet ef database update \
  --project src/ClientlyMan.Infrastructure/ClientlyMan.Infrastructure.csproj \
  --startup-project src/ClientlyMan.Api/ClientlyMan.Api.csproj
```

When running via Aspire, the API uses the orchestrated PostgreSQL instance and applies migrations automatically on first use (ensure the database user has sufficient permissions).

## Testing

Run all tests from the solution root:
```bash
dotnet test
```

- `ClientlyMan.Tests.Unit` exercises application services with mocks.
- `ClientlyMan.Tests.Integration` uses `WebApplicationFactory` with an in-memory database to verify API endpoints end-to-end.

## API overview

All endpoints are rooted at `/api` and documented via Swagger.

### Customers
- `GET /api/customers?name=&taxNumber=` – list customers with optional filters.
- `GET /api/customers/{id}` – retrieve a single customer.
- `POST /api/customers` – create a customer.
- `PUT /api/customers/{id}` – update a customer.
- `DELETE /api/customers/{id}` – remove a customer.
- `GET /api/customers/{id}/policies` – list policies for a customer.
- `GET /api/customers/{id}/simulations` – list simulations for a customer.

### Policies
- `GET /api/policies` – list policies.
- `GET /api/policies/{id}` – retrieve a policy.
- `POST /api/policies` – create a policy.
- `PUT /api/policies/{id}` – update a policy.
- `DELETE /api/policies/{id}` – delete a policy.
- `GET /api/policies/expiring?days=30` – list policies ending within the next `days`.

### Simulations
- `GET /api/simulations` – list simulations.
- `GET /api/simulations/{id}` – retrieve a simulation.
- `POST /api/simulations` – create a simulation request.
- `PUT /api/simulations/{id}` – update a simulation.
- `DELETE /api/simulations/{id}` – delete a simulation.

HTTP status codes follow REST conventions: `200`/`201` on success, `204` for updates/deletes, `400` for validation issues, and `404` when resources are missing.

## Adding new features

1. **Domain** – extend entities or add new ones.
2. **Application** – create DTOs, validators, services, and repository interfaces for new use cases.
3. **Infrastructure** – implement repository logic, update EF configurations, and create migrations.
4. **API** – expose the new functionality via controllers/endpoints.
5. **Tests** – add unit and integration coverage for new behaviors.
6. Update documentation/README if necessary.

## Tech stack

- .NET 9 (C# 13 preview)
- ASP.NET Core Web API
- .NET Aspire orchestration
- Entity Framework Core with PostgreSQL provider
- FluentValidation
- Swashbuckle (Swagger/OpenAPI)
- xUnit, Moq, FluentAssertions for testing
