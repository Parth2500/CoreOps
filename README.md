# CoreOps

CoreOps is a multi-tenant workflow engine for internal requests. A tenant defines workflows as states and allowed transitions (for example `draft → submitted → approved | rejected`). Users file requests against a workflow, and each request moves through it. Every move is kept in a state history, and every change can be written to an audit log.

**Status:** on hold since January 2026. The domain model, schema and first migration are done. Two endpoints work: create a request and read it by id. Moving a request between states, managing workflows, auth and the front end are not built. See [Roadmap](docs/roadmap.md).

## Stack

- .NET 9, ASP.NET Core controllers, Swagger in Development
- SQL Server 2022 in Docker, EF Core 9
- Clean Architecture (dependencies point inward only), hand-written handlers (no MediatR)
- Serilog
- xUnit, Moq, FluentAssertions
- Multi-tenant from the first migration: every row carries a `TenantId`

## Repository layout

| Path | Content |
|---|---|
| `backend/CoreOps.Domain` | Entities. No dependencies. |
| `backend/CoreOps.Application` | Use cases (commands, queries, handlers, validators) and repository interfaces. |
| `backend/CoreOps.Infrastructure` | EF Core `DbContext`, entity configurations, migrations, repositories. |
| `backend/CoreOps.Api` | Controllers, request/response contracts, exception middleware, composition root. |
| `backend/CoreOps.Tests` | Unit tests for the Application handlers. No database. |
| `docker/` | `docker-compose.yml` for SQL Server. |
| `frontend/` | Empty. Reserved for the UI. |
| `docs/` | Domain model, schema, roadmap. |

## Documents

- [Domain model](docs/domain-model.md): the entities, what each one means, and how a request moves through a workflow.
- [Schema](docs/schema.md): tables, schemas, keys and indexes as the `InitialCreate` migration builds them.
- [Roadmap](docs/roadmap.md): what is done, what comes next, and known issues.

## Build and test

You need a .NET SDK that can target .NET 9 (SDK 9 or 10). The tests need the .NET 9 runtime. With only the .NET 10 runtime installed, let them roll forward:

```
cd backend
dotnet build CoreOps.sln
DOTNET_ROLL_FORWARD=Major dotnet test CoreOps.sln
```

## Run locally

You need Docker (or Podman) for SQL Server.

1. Start SQL Server:
   ```
   docker compose -f docker/docker-compose.yml up -d
   ```
   The `sa` password is `YourStrong!Passw0rd`. It is a development value, also in `appsettings.Development.json`. Change both for anything that is not your own machine.

2. Apply the migrations:
   ```
   cd backend
   dotnet tool install --global dotnet-ef   # once
   dotnet ef database update --project CoreOps.Infrastructure --startup-project CoreOps.Api
   ```

3. Start the API:
   ```
   dotnet run --project CoreOps.Api --launch-profile http
   ```
   Swagger is at http://localhost:5201/swagger.

4. There is no endpoint to create workflows yet, so insert one by hand before creating a request:
   ```sql
   INSERT INTO workflow.WorkflowDefinitions (Id, TenantId, Name, IsActive, CreatedBy, CreatedAt)
   VALUES ('11111111-1111-1111-1111-111111111111', '00000000-0000-0000-0000-0000000000a1',
           'Leave request', 1, '00000000-0000-0000-0000-0000000000b1', SYSUTCDATETIME());
   ```
   Then:
   ```
   curl -X POST http://localhost:5201/api/requests -H 'Content-Type: application/json' -d '{
     "tenantId": "00000000-0000-0000-0000-0000000000a1",
     "title": "Two days off in March",
     "description": null,
     "workflowDefinitionId": "11111111-1111-1111-1111-111111111111",
     "initialStateCode": "draft",
     "createdBy": "00000000-0000-0000-0000-0000000000b1"
   }'
   curl http://localhost:5201/api/requests/<requestId>
   ```

## API

| Method | Path | Result |
|---|---|---|
| `POST` | `/api/requests` | `200` with `{ requestId }`. `400` with a list of errors if validation fails. `404` if the workflow definition does not exist. |
| `GET` | `/api/requests/{id}` | `200` with the request. `404` if it does not exist. |

Errors come from `ExceptionHandlingMiddleware` as `{ "message": ..., "errors": [...] }`. Any other exception falls through to ASP.NET Core's default handling.

## Related projects

[Heimdall](https://github.com/Parth2500/heimdall-idp) is the natural identity provider for CoreOps. Its tokens would supply the tenant and the user, which CoreOps currently takes from the request body.
