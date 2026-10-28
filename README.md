# Ticket Management System

An open-source service-desk and ticketing platform being rebuilt as a production-grade reference application for customer support, internal operations, and IT service management.

> **Status:** V2 architecture migration in progress. The existing Blazor/.NET 8 application remains in the repository as a legacy behavior reference while the new API-first platform is introduced incrementally.

## Product direction

The V2 platform is designed around more than CRUD tickets. The target product model includes:

- ticket lifecycle and auditable history;
- customer organizations, requesters, agents, teams, roles, and permissions;
- configurable queues and saved views;
- SLA policies, business calendars, breach detection, and escalations;
- manual, round-robin, load-aware, and skill-based routing;
- customer replies, private notes, mentions, followers, and attachments;
- trigger/condition/action automations;
- service catalog and dynamic request forms;
- approvals and reusable workflows;
- knowledge base and self-service;
- incident, problem, and change-management capabilities;
- notifications, email ingestion, webhooks, and external integrations;
- operational analytics, CSAT, and SLA reporting.

## Architecture direction

V2 follows an API-first modular-monolith architecture with strict dependency boundaries:

```text
Angular Web Client
       |
       v
ASP.NET Core API
       |
       v
Application Use Cases
       |
       v
Domain Model
       ^
       |
Infrastructure Adapters
```

The planned runtime foundation is:

- .NET 10 LTS;
- ASP.NET Core;
- Angular 22;
- PostgreSQL;
- Redis where distributed coordination/caching is justified;
- .NET Aspire for local orchestration and observability;
- OpenTelemetry;
- Docker;
- Testcontainers and Playwright for end-to-end confidence.

The system starts as a modular monolith. Microservices are not a goal; a module should be extracted only when operational or scaling evidence justifies the additional distributed-system cost.

## Repository layout

```text
Domain/                 # Legacy V1 domain/data contracts
Infrastructure/         # Legacy V1 persistence/services
TicketManagementUI/     # Legacy V1 Blazor Server UI

src/                    # V2 production source (introduced incrementally)
tests/                  # V2 automated tests
docs/                   # Architecture, decisions, roadmap, migration notes
```

## V1 security note

The historical V1 application previously contained development-only configuration and credential notes in source control. The V2 migration starts with a public-repository safety pass: secrets are removed from active configuration and must be supplied through environment variables, .NET user-secrets, or a production secret provider.

Never commit reusable keys or production connection strings. See [SECURITY.md](SECURITY.md).

## Migration principles

1. Preserve useful V1 business behavior as a reference, not as an architectural constraint.
2. Do not introduce a second competing implementation of the same V2 capability.
3. Keep the domain independent of UI, EF Core, ASP.NET Identity, and transport concerns.
4. Model ticket lifecycle changes as business operations rather than arbitrary property mutation.
5. Make tenant/workspace isolation explicit from the start.
6. Use an outbox for durable publication of events that drive automation or integration side effects.
7. Prefer idempotent handlers and observable background processing.
8. Build security, testing, and observability into each phase rather than adding them at the end.

## Roadmap

The high-level implementation sequence is documented in [docs/roadmap/ROADMAP.md](docs/roadmap/ROADMAP.md).

Architecture boundaries and module ownership are documented in [docs/architecture/ARCHITECTURE.md](docs/architecture/ARCHITECTURE.md).

## Contributing

The project is currently in foundation work. Changes should be small, reviewable, covered by the most relevant tests, and consistent with the architecture decision records under `docs/architecture`.

Before adding a new dependency or infrastructure component, document why the capability cannot be satisfied cleanly by the existing platform.

## License

A repository license will be selected before the first V2 public release. Until then, no additional rights are granted beyond GitHub's standard repository viewing/forking terms.
