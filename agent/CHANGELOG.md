# CHANGELOG

> Dated milestones. Newest first.

## [2026-06-08] Remaining modules + end-to-end verification

- Frontend: completed Technicians, Spare Parts, and Settings modules (users,
  roles, categories, locations) with a settings sub-nav and role-filtered
  navigation. Removed the deprecated TypeScript `baseUrl` option and installed
  the missing rolldown native binding so `pnpm build` passes cleanly.
- Backend: added read/CRUD controllers for Equipment, Dashboard, Schedule,
  Technician, SparePart, Alert, Reports, and Settings so the UI works
  end-to-end. WorkOrder stays the full CQRS/MediatR reference module.
- Fixed a `GroupBy(...).Date` translation failure in the dashboard work-order
  trend by grouping the timestamps in memory.
- Verified every endpoint against the seeded PostgreSQL database via JWT login.

## [2026-06-08] Initial full-stack scaffold

- Created `agent/` context folder (AI_CONTEXT, DECISIONS, PROGRESS, TODO, CHANGELOG).
- Frontend: renamed `fe/todo` → `fe/cmms`, added CMMS dependencies, Tailwind CSS +
  shadcn-style primitives, feature-based folder structure, providers (TanStack
  Query, Router, Sonner), Axios client with auth/error interceptors, SignalR
  client, Zustand stores, TanStack Query hooks, route map with lazy loading and
  role guards, and scaffolded pages for every module.
- Backend: ASP.NET Core 8 Clean Architecture solution (Domain, Application,
  Infrastructure, Api). Domain entities + enums + audit/soft-delete base classes.
  EF Core 8 + Npgsql DbContext mapping the Liquibase schema. JWT auth with
  role-based policies, global exception middleware, Serilog. WorkOrder CQRS
  (commands/queries/validators/handlers) as the reference module + controller.
  Hangfire PM auto-generation job. SignalR `WorkOrderHub`.
- Database: `docker-compose.yml` (PostgreSQL 16 + Liquibase runner + pgAdmin),
  Liquibase `001-initial-schema.sql` and `002-seed-data.sql` (PostgreSQL dialect,
  no JSONB).
- Docs: OpenAPI 3.1 `cmms-api.yaml` with per-module example JSON files; Swashbuckle
  wired to serve Swagger UI at `/swagger`.
