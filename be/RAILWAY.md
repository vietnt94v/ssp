# Deploy CMMS API to Railway

## Railway setup

1. Create a project on [Railway](https://railway.app).
2. **New → GitHub Repo** → select `vietnt94v/ssp`.
3. Service **Settings → Source**:
   - **Root Directory:** `be`
   - **Branch:** `main`
   - Auto-deploy: enabled
4. **New → Database → PostgreSQL** in the same project.
5. API service → **Variables** → add references from the Postgres service.

Railway detects `be/Dockerfile` automatically when the root directory is `be`.

## Required environment variables

Set these on the **API** service (Variables tab):

| Variable | Example |
|----------|---------|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ConnectionStrings__Default` | `Host=${{Postgres.PGHOST}};Port=${{Postgres.PGPORT}};Database=${{Postgres.PGDATABASE}};Username=${{Postgres.PGUSER}};Password=${{Postgres.PGPASSWORD}};SSL Mode=Require;Trust Server Certificate=true` |
| `Jwt__SigningKey` | Generate with `openssl rand -base64 48` |
| `Cors__AllowedOrigins` | `https://your-app.vercel.app,http://localhost:5173` |

Replace `Postgres` in variable references if your database service has a different name.

Optional JWT overrides:

| Variable | Default |
|----------|---------|
| `Jwt__Issuer` | `Ssp.Cmms` |
| `Jwt__Audience` | `Ssp.Cmms.Client` |

## Database migrations

Run Liquibase once against the production database before using the API:

```bash
liquibase \
  --changelog-file=liquibase/changelog-master.yaml \
  --url="jdbc:postgresql://HOST:PORT/DATABASE" \
  --username=USER \
  --password=PASSWORD \
  --driver=org.postgresql.Driver \
  update
```

Run from the repo `db/` directory. Use the public Postgres connection values from Railway.

## Vercel (frontend)

After the API is live, copy the Railway public URL and set on Vercel:

| Variable | Value |
|----------|-------|
| `VITE_API_URL` | `https://<railway-host>/api` |
| `VITE_SIGNALR_URL` | `https://<railway-host>/hubs/work-orders` |

Redeploy Vercel after updating variables.

## Verify

- Health: open `https://<railway-host>/api/auth/login` (expect 405 or validation error, not connection refused).
- Login from the Vercel app with a seeded user after migrations complete.

## Local Docker test

From the repo root:

```bash
docker build -t cmms-api ./be
docker run --rm -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e ConnectionStrings__Default="Host=host.docker.internal;Port=5434;Database=cmms;Username=cmms;Password=cmms_dev" \
  -e Jwt__SigningKey="dev-only-super-secret-signing-key-change-me-32+chars" \
  cmms-api
```

API listens on http://localhost:8080.
