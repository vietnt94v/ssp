# PROGRESS

> Current state of the build. Update after each work session.

## Status legend

`Not started` · `In progress` · `Done`

## Phases

| Phase | Description | Status |
| --- | --- | --- |
| 0 | Agent context folder | Done |
| 1A | Rename FE + dependencies + Tailwind/shadcn | Done (scaffold) |
| 1B | FE folder structure | Done |
| 1C | BE Clean Architecture solution | Done |
| 1D | Docker PostgreSQL + Liquibase | Done |
| 1E | Liquibase schema + seed | Done |
| 1F | OpenAPI spec + Swagger | Done |
| 2 | Domain model (FE types + BE entities) | Done |
| 3 | API design (endpoints, WO CQRS, Hangfire, SignalR, JWT) | Done |
| 4 | FE routing + role guards | Done |
| 5 | Screens by role | Done |
| 6 | State management (stores + query hooks) | Done |
| 7 | BE module controllers (Equipment, Dashboard, Schedule, Technician, SparePart, Alert, Reports, Settings) | Done |
| 8 | End-to-end verification against seeded PostgreSQL | Done |

## Notes / current focus

- Full stack is running end-to-end: FE builds cleanly, BE builds with 0 errors,
  API verified against the seeded PostgreSQL database (login + every module
  endpoint returns 200).
- .NET 8 SDK is installed under `~/.dotnet`; PostgreSQL runs via Docker on host
  port 5434.
- WorkOrder remains the full CQRS/MediatR reference; the other modules use thin
  controllers over the DbContext, ready to promote to CQRS as logic grows.

## Known follow-ups

- Promote remaining controllers to CQRS handlers where business logic grows.
- Replace placeholder OEE/MTBF calculations once plant formulas are confirmed.
- File upload currently a placeholder (no cloud storage).
