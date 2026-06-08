# AI_CONTEXT

> Read this file FIRST in every session, then DECISIONS.md, PROGRESS.md, TODO.md.
> Do not propose solutions that conflict with DECISIONS.md. Continue from PROGRESS.md.
> If information is missing, ask before assuming.

## What is this project

CMMS (Computerized Maintenance Management System) for manufacturing plants.
A maintenance team uses it to register equipment, raise and track work orders,
schedule preventive maintenance, assign technicians, track costs, and view KPIs.

## Modules

- Equipment Registry — assets, category, location, status
- Work Order Management — corrective / preventive / inspection tickets
- Preventive Maintenance Schedule — recurring rules, calendar, upcoming alerts
- Technician Assignment — ownership and workload
- Cost Tracking — parts and labor cost per work order
- Maintenance History — per-equipment timeline
- Dashboard & KPIs — MTTR, MTBF, OEE impact, open WOs, overdue
- Spare Parts — inventory / parts used per WO
- Alerts — PM due, breakdown, WO overdue, low stock
- Settings — users, roles, categories, locations (Admin only)

## Repository map

```
ssp/
├── agent/              # AI context (this folder)
├── docker-compose.yml  # PostgreSQL + Liquibase (+ optional pgAdmin)
├── fe/cmms/            # React 19 + TypeScript (Vite) frontend
├── be/                 # ASP.NET Core 8 Clean Architecture API
│   ├── docs/openapi/   # OpenAPI 3.1 spec + JSON examples
│   ├── src/Ssp.Cmms.Domain
│   ├── src/Ssp.Cmms.Application
│   ├── src/Ssp.Cmms.Infrastructure
│   └── src/Ssp.Cmms.Api
└── db/liquibase/       # SQL changesets (schema + seed)
```

## Tech stack

### Frontend (`fe/cmms`)

| Layer | Library |
| --- | --- |
| Framework | React 19 + TypeScript (strict) + Vite |
| Server state | TanStack Query v5 |
| Client state | Zustand |
| Routing | React Router v6 (lazy loading) |
| Forms | React Hook Form + Zod |
| HTTP | Axios (interceptors: auth + error) |
| Dates | date-fns |
| Charts | Recharts |
| Calendar | FullCalendar (react) |
| Tables | TanStack Table v8 |
| UI | shadcn/ui + Tailwind CSS |
| Notifications | Sonner |
| Real-time | @microsoft/signalr |
| Export | ExcelJS (.xlsx) + jsPDF (PDF) |

### Backend (`be/`)

| Layer | Library |
| --- | --- |
| API | ASP.NET Core 8 Web API (API only) |
| ORM | EF Core 8 + Npgsql (maps Liquibase schema; no EF migrations) |
| DB | PostgreSQL 16 (Docker) |
| Mapping | AutoMapper |
| Validation | FluentValidation |
| CQRS | MediatR (Commands / Queries) |
| Jobs | Hangfire (Hangfire.PostgreSql) |
| Real-time | SignalR |
| Auth | JWT Bearer, role-based policies |
| Logging | Serilog |
| Docs | Swagger / OpenAPI (Swashbuckle + examples) |

## Roles

`Admin` | `Manager` | `Technician`

- Admin: full access (including Settings)
- Manager: work orders, maintenance calendar, reports, equipment, technicians, spare parts
- Technician: view own tasks, update progress (read-only schedule)

## Work Order status machine

```
Draft → Assigned → InProgress → OnHold → Completed → Closed
```

`OnHold` can return to `InProgress`. Closed is terminal. Transitions are
enforced server-side in `ChangeWorkOrderStatusCommand`.

## Local dev commands

```bash
# Database (PostgreSQL + Liquibase migrations + seed)
docker compose up -d

# Backend API (after .NET 8 SDK installed)
cd be/src/Ssp.Cmms.Api && dotnet run    # http://localhost:5000, Swagger at /swagger

# Frontend
cd fe/cmms && pnpm install && pnpm dev   # http://localhost:5173
```

Default dev connection string (host port 5434 — 5432 was taken by another
local container, so Compose maps `5434:5432`):
`Host=localhost;Port=5434;Database=cmms;Username=cmms;Password=cmms_dev`

## Seed accounts (dev only)

| Email | Role | Password |
| --- | --- | --- |
| admin@cmms.local | Admin | Passw0rd! |
| manager@cmms.local | Manager | Passw0rd! |
| tech@cmms.local | Technician | Passw0rd! |
