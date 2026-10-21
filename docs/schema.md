# Schema

SQL Server. One migration so far: `20251227140540_InitialCreate`. Mappings are in `backend/CoreOps.Infrastructure/Persistence/Configurations/`, one class per entity, applied with `ApplyConfigurationsFromAssembly`.

All ids are `uniqueidentifier`, generated in the domain constructors (except `User.Id`, which is passed in). All timestamps are UTC `datetime2`, set with `DateTime.UtcNow`.

## `workflow`

| Table | Columns | Keys and indexes |
|---|---|---|
| `WorkflowDefinitions` | `Id`, `TenantId`, `Name` (200), `Description` (500, null), `IsActive`, `CreatedBy`, `CreatedAt` | PK `Id` |
| `WorkflowStates` | `Id`, `TenantId`, `WorkflowDefinitionId`, `Code` (100), `DisplayName` (200), `IsInitial`, `IsTerminal`, `SortOrder` | PK `Id`. FK to `WorkflowDefinitions`, cascade delete. Unique (`WorkflowDefinitionId`, `Code`). |
| `WorkflowTransitions` | `Id`, `TenantId`, `WorkflowDefinitionId`, `FromStateId`, `ToStateId`, `RequiresComment`, `IsAutomatic` | PK `Id`. FK to `WorkflowDefinitions`, cascade delete. Unique (`WorkflowDefinitionId`, `FromStateId`, `ToStateId`). |

## `operations`

| Table | Columns | Keys and indexes |
|---|---|---|
| `Requests` | `Id`, `TenantId`, `Title` (200), `Description` (max, null), `WorkflowDefinitionId`, `CurrentStateCode` (100), `CreatedBy`, `CreatedAt`, `LastModifiedAt` | PK `Id`. Index on `CurrentStateCode`. |
| `RequestStateHistory` | `Id`, `TenantId`, `RequestId`, `FromStateCode` (100, null), `ToStateCode` (100), `TransitionedBy`, `TransitionedAt`, `Comment` (500, null) | PK `Id`. Index (`RequestId`, `TransitionedAt`). |

## `identity`

| Table | Columns | Keys and indexes |
|---|---|---|
| `Users` | `Id`, `TenantId`, `Email` (256), `DisplayName` (200), `IsActive` | PK `Id`. Unique (`TenantId`, `Email`). |
| `Roles` | `Id`, `TenantId`, `Code` (100), `Name` (200), `IsActive` | PK `Id`. Unique (`TenantId`, `Code`). |
| `UserRoles` | `UserId`, `RoleId` | PK (`UserId`, `RoleId`). |

## `audit`

| Table | Columns | Keys and indexes |
|---|---|---|
| `AuditLogs` | `Id`, `TenantId`, `EntityType` (100), `EntityId`, `Action` (100), `PerformedBy`, `PerformedAt`, `Data` (max) | PK `Id` |

## Gaps in the current schema

These are deliberate omissions or oversights to settle before the next migration. [Roadmap](roadmap.md) tracks them.

- **Missing foreign keys.** Only states and transitions reference their workflow. These have no FK:
  - `WorkflowTransitions.FromStateId` / `ToStateId` → `WorkflowStates`
  - `Requests.WorkflowDefinitionId` → `WorkflowDefinitions`
  - `RequestStateHistory.RequestId` → `Requests`
  - `UserRoles.UserId` / `RoleId` → `Users` / `Roles`
  - `CreatedBy`, `TransitionedBy`, `PerformedBy` → `Users` (may stay unenforced if users come from an external IdP)
- **`TenantId` never set on states and transitions.** Their constructors do not take a tenant, so the column is always `Guid.Empty`. Either pass the workflow's tenant in, or drop the column and rely on the parent workflow.
- **No tenant-leading indexes.** Lists of requests will filter by tenant first. `Requests` needs an index such as (`TenantId`, `CurrentStateCode`) instead of the `CurrentStateCode` index alone.
- **No concurrency token.** Two transitions on the same request at once could both succeed. Add a `rowversion` column on `Requests` before transitions are built.
