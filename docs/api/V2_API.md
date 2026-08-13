# TicketManagementSystem V2 API

Status: foundation contract. Endpoints will evolve behind `/api/v2` while the V1 application remains a migration reference.

## Tenant context

During the pre-identity foundation phase, ticket-scoped endpoints resolve the current workspace from the `X-Workspace-Id` request header.

This header is intentionally temporary. Once V2 Identity is introduced, the trusted workspace context must come from authenticated server-side claims/membership resolution. Client-supplied workspace identifiers must not become the authorization boundary.

## System

### `GET /health`

Liveness/readiness entry point for the API host.

### `GET /api/v2/system`

Returns basic service/version/time information.

## Workspaces

### `POST /api/v2/workspaces`

Creates a workspace.

Request:

```json
{
  "name": "Acme Support",
  "slug": "acme-support"
}
```

Rules:

- name is required;
- slug is normalized to lowercase;
- slug may contain letters, numbers, and hyphens;
- slug is unique at both application and database levels;
- new workspaces start active.

## Tickets

### `POST /api/v2/tickets`

Requires `X-Workspace-Id` during the foundation phase.

Request:

```json
{
  "requesterId": "019c0000-0000-7000-8000-000000000001",
  "summary": "Printer is unavailable",
  "description": "The office printer stopped accepting jobs.",
  "priority": "High"
}
```

The application layer verifies that the current workspace exists and is active. The Domain creates the Ticket and owns lifecycle invariants.

### `GET /api/v2/tickets/{ticketId}`

Requires `X-Workspace-Id` during the foundation phase.

The persistence layer applies workspace isolation automatically. A ticket belonging to another workspace is not returned through the scoped repository.

## Error model

The API uses RFC-style Problem Details for unhandled HTTP errors. Domain-rule violations are being standardized as `422 Unprocessable Entity` responses as part of the API error-handling foundation.

## Design rule

HTTP endpoints orchestrate application use cases. They do not contain ticket business rules and do not access EF Core directly.
