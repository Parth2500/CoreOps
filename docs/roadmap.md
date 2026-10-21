# Roadmap

Written in October 2026, while the project has been on hold since January 2026. It describes the code as it stands and what to build next, in order.

## Done

- Solution structure: Domain, Application, Infrastructure, Api, Tests.
- Domain model for workflows, requests, identity and audit ([Domain model](domain-model.md)).
- EF Core configurations and the `InitialCreate` migration ([Schema](schema.md)).
- `POST /api/requests`: validates the input, rejects an unknown workflow with 404, saves the request.
- `GET /api/requests/{id}`: returns the request, or 404.
- Exception middleware: `ValidationException` → 400, `NotFoundException` → 404.
- Handler unit tests for both endpoints (5 tests, passing).
- Serilog request logging, Swagger in Development, Dockerfile for the API.

## Next

1. **Workflow management.** Add methods to `WorkflowDefinition` for adding states and transitions, and enforce the rules there: exactly one initial state, at least one terminal state, transitions only between states of the same workflow. Add `POST /api/workflows` (states and transitions in one body), `GET /api/workflows/{id}`, and activate/deactivate.
2. **Fix request creation.** Take the initial state from the workflow instead of the request body. Refuse inactive workflows. Write the first `RequestStateHistory` row (`FromStateCode = null`).
3. **Transitions.** Add `POST /api/requests/{id}/transitions` with `{ toStateCode, comment }`. Check that the transition exists, enforce `RequiresComment`, refuse requests already in a terminal state, and write history and audit rows in one transaction. Add a `rowversion` column on `Requests` first.
4. **List and history.** `GET /api/requests` filtered by state and paged, and `GET /api/requests/{id}/history`.
5. **Auth with Heimdall.** Validate Heimdall access tokens with JWT bearer. Take `TenantId` and the user id from the token, and remove them from request bodies. Add an EF global query filter on `TenantId`.
6. **Authorization rules.** Decide who may make each transition, probably by role on `WorkflowTransition`.
7. **Front end** in `frontend/`.

## Known issues

| Issue | Where | Effect |
|---|---|---|
| Tenant and user come from the request body | `CreateRequestRequest` | Any caller can act as any tenant or user. Fixed by step 5. |
| `GET /api/requests/{id}` does not filter by tenant | `RequestRepository.GetByIdAsync` | Any caller can read any tenant's request by id. Fixed by step 5. |
| Initial state comes from the caller and is not checked | `CreateRequestHandler` | A request can start in a state that does not exist. Fixed by step 2. |
| `TenantId` is never set on states and transitions | `WorkflowState`, `WorkflowTransition` | The column is always `Guid.Empty`. See [Schema](schema.md#gaps-in-the-current-schema). |
| Missing foreign keys | Migration | Orphaned rows are possible. See [Schema](schema.md#gaps-in-the-current-schema). |
| Unhandled exceptions are not mapped | `ExceptionHandlingMiddleware` | Anything other than the two known exceptions falls through to the default 500 page. Consider `ProblemDetails` for every error. |
| SQL Server health check uses the old tools path | `docker/docker-compose.yml` | Current `mssql/server:2022-latest` images ship `sqlcmd` in `/opt/mssql-tools18/bin` and need `-C`, so the check may report the container unhealthy. |
| `sa` password is committed | `docker-compose.yml`, `appsettings.Development.json` | Fine for local development only. Move it to `.env` and user secrets, as Heimdall does. |
| `CoreOps.Api.http` still calls `/weatherforecast` | `CoreOps.Api.http` | Leftover from the template. |
| No integration tests | `CoreOps.Tests` | Repositories and EF mappings are untested. Testcontainers with SQL Server would cover them. |
