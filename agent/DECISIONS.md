# DECISIONS

> Architectural decisions that are LOCKED. Do not propose anything that conflicts
> with these. Append new decisions; do not rewrite history.

## ADR-001 — Database: PostgreSQL via Docker

PostgreSQL 16 runs in Docker Compose for local dev (not SQL Server, despite the
original BE spec). EF Core uses the Npgsql provider.

## ADR-002 — Schema owned by Liquibase, not EF migrations

Schema lives in `db/liquibase` as SQL changesets. EF Core maps the existing
schema; we do NOT run `dotnet ef migrations`. The Liquibase runner applies
changesets before the API connects.

## ADR-003 — No JSONB columns

Workspace rule. Use normalized tables and `VARCHAR`/`TEXT` columns. Enum values
are stored as `VARCHAR`. Lists (checklist, parts) are separate tables.

## ADR-004 — OpenAPI spec committed with examples

`be/docs/openapi/cmms-api.yaml` (OpenAPI 3.1) is the documented contract, with
per-module request/response examples under `be/docs/openapi/examples/`.
Swashbuckle serves Swagger UI at `/swagger` and is configured with example filters.

## ADR-005 — Real-time via SignalR (not polling)

A single SignalR hub pushes work order status changes, alerts, and dashboard KPI
updates. The frontend subscribes on load and invalidates TanStack Query caches.

## ADR-006 — Notifications via Sonner

Toasts use Sonner (pairs cleanly with shadcn/ui).

## ADR-007 — Frontend feature-based folders

`fe/cmms/src/features/<module>/{api,hooks,components,pages,schemas}`. Shared UI
in `components/`, shared logic in `lib/`, client state in `stores/`.

## ADR-008 — Backend Clean Architecture

Domain → Application → Infrastructure → Api. Dependencies point inward.
CQRS with MediatR. Repository + Unit of Work in Infrastructure.

## ADR-009 — Auth: JWT Bearer + role policies

Roles: Admin, Manager, Technician. Policies: `RequireAdmin`,
`RequireManagerOrAdmin`, `RequireTechnicianOrAbove`.

## ADR-010 — Server state vs client state

Server data lives in TanStack Query. Zustand holds only UI/client state
(filters, modals, sidebar, unread alert count, auth token).

## ADR-011 — Soft delete on Equipment and WorkOrder

`is_deleted`, `deleted_at`, `deleted_by`. EF global query filters exclude
soft-deleted rows by default.

## ADR-012 — Audit fields on all entities

`created_at`, `created_by`, `updated_at`, `updated_by`, set via an EF
`SaveChanges` interceptor using the current user.

## ADR-013 — React 19 kept

The existing Vite scaffold uses React 19. We keep it; all required libraries are
compatible with React 18 patterns used here.
