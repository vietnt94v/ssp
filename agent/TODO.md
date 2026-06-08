# TODO

> Ordered backlog. Move items to "Done" as completed; add discovered tasks.

## Done

- [x] Agent context folder (AI_CONTEXT, DECISIONS, PROGRESS, TODO, CHANGELOG)
- [x] Rename fe/todo → fe/cmms, install deps, Tailwind + shadcn setup
- [x] FE folder structure + providers + axios + SignalR client
- [x] FE routing with lazy loading + role guards
- [x] FE Zustand stores + TanStack Query hooks per module
- [x] FE screens scaffolded per module (by role)
- [x] BE Clean Architecture solution (Domain/Application/Infrastructure/Api)
- [x] BE Domain entities, enums, audit + soft-delete base classes
- [x] BE EF Core DbContext + Npgsql mapping
- [x] BE JWT auth + role policies + global exception middleware + Serilog
- [x] BE WorkOrder MediatR commands/queries (reference) + controller
- [x] BE Hangfire PM generation job + WorkOrderHub SignalR
- [x] docker-compose.yml (PostgreSQL + Liquibase + pgAdmin)
- [x] Liquibase 001-initial-schema.sql + 002-seed-data.sql
- [x] OpenAPI cmms-api.yaml + example JSON per module
- [x] Install .NET 8 SDK; `dotnet restore && dotnet build` the solution
- [x] `docker compose up -d`; verify migrations + seed applied
- [x] FE Technicians, Spare Parts, Settings modules + role-filtered nav
- [x] BE read/CRUD controllers: Equipment, Dashboard, Schedule, Technician,
      SparePart, Alert, Reports, Settings (WorkOrder remains full CQRS reference)
- [x] Verified all endpoints end-to-end against seeded PostgreSQL

## Next

- [ ] Promote remaining controllers to full CQRS handlers where business logic grows
- [ ] FE: implement export to PDF/Excel actions end-to-end
- [ ] Confirm OEE/MTBF formulas; implement real KPI calculations
- [ ] File upload (equipment images, WO photos) backing store
- [ ] Tests (Application handlers; FE component/integration) — later phase
