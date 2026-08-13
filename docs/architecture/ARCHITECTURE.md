# V2 Architecture

## 1. Architectural goals

The V2 platform is a production-grade, API-first service desk built as a modular monolith.

The architecture optimizes for:

- explicit business rules and lifecycle invariants;
- independently understandable business modules;
- strong tenant/workspace isolation;
- durable automation/integration side effects;
- testability without infrastructure;
- observability and operability;
- future extraction of modules only when justified.

## 2. Dependency rule

Dependencies point inward:

```text
Web/API/Workers
      |
      v
Application
      |
      v
Domain

Infrastructure ----> Application/Domain contracts
```

The Domain project must not reference:

- EF Core;
- ASP.NET Core;
- ASP.NET Identity;
- Angular/UI concerns;
- serialization/transport DTOs;
- database-specific types;
- logging providers;
- message brokers.

Application owns use cases and ports. Infrastructure implements ports. Entry-point projects compose the application.

## 3. Initial module map

### Identity & Access

Authentication identity mapping, roles, permissions, sessions, and authorization policy inputs.

### Organizations

Workspaces/tenants, customer organizations, contacts/requesters, support plans, and membership.

### Tickets

Ticket aggregate, lifecycle, priority, category, tags, relationships, watchers, resolution metadata, and domain events.

### Conversations

Public replies, internal notes, system timeline entries, mentions, and participant/follower behavior.

### Attachments

Attachment metadata and authorization. Binary storage is behind an application port.

### Queues

Saved queue definitions, filter expressions, visibility, ordering, and queue ownership.

### SLA

Policies, targets, business calendars, pause/resume rules, deadlines, breach state, and escalation triggers.

### Routing

Assignment strategies, team eligibility, agent capacity, skills, fallback behavior, and routing decisions.

### Automation

Trigger/condition/action definitions, rule evaluation, execution logs, retries, and idempotency.

### Catalog

Request types, service catalog items, dynamic forms, field definitions, field values, and conditional form rules.

### Approvals

Approval requests, approvers, sequential/parallel strategies, decisions, expiry, and reminders.

### Knowledge

Articles, categories, publishing state, search metadata, ticket/article relationships, and self-service suggestions.

### ITSM

Incidents, problems, changes, service relationships, major-incident coordination, RCA, and change approvals.

### Notifications

Notification intents, templates, preferences, email/in-app adapters, and delivery tracking.

### Integrations

Inbound email, webhooks, external connectors, API credentials, and integration delivery history.

### Analytics

Operational read models and reporting projections. Analytics must not become a second source of truth for transactional business state.

### Audit

Immutable business/security audit records for sensitive actions and workflow changes.

## 4. Ticket aggregate boundary

The Ticket aggregate is the source of truth for ticket lifecycle state.

Expected operations include:

- create;
- assign/reassign;
- change priority/category;
- start progress;
- wait for requester/internal/third party;
- resolve;
- close;
- reopen;
- cancel;
- mark duplicate/link related tickets.

The V2 domain must not expose arbitrary public setters that bypass lifecycle rules.

A status transition must be validated by the domain or by a workflow policy invoked through the application layer.

## 5. State and workflow strategy

V2 starts with a canonical built-in workflow so the product is useful before a workflow designer exists.

Default conceptual states:

```text
New
Triage
Assigned
InProgress
PendingRequester
PendingInternal
PendingThirdParty
Resolved
Closed
Reopened
Cancelled
Duplicate
```

Custom workflow definitions are a later capability. They must not create multiple competing fields for the canonical ticket lifecycle.

## 6. Tenant/workspace isolation

All tenant-owned aggregates carry a WorkspaceId/TenantId value in their domain identity/context.

Rules:

- tenant scope is established at the application boundary;
- queries must apply tenant scope explicitly or through a verified global mechanism;
- commands must verify referenced resources belong to the same tenant;
- cache keys include tenant identity;
- background jobs carry tenant identity as part of the durable message/job payload;
- tests must include cross-tenant access attempts.

## 7. Data architecture

PostgreSQL is the target primary transactional database.

Rules:

- EF Core mappings live in Infrastructure;
- entities do not contain EF mapping attributes;
- read-heavy dashboards may use dedicated projections;
- projections are disposable/rebuildable and never authoritative for business writes;
- concurrency-sensitive aggregates use optimistic concurrency where appropriate;
- UTC is used for persisted instants; business calendars/time zones are explicit domain concepts.

## 8. Eventing and outbox

Domain events represent facts that occurred inside a successful business operation.

Integration/automation side effects are published durably through an outbox stored in the same database transaction as the business write.

Expected flow:

```text
Command
  -> Aggregate operation
  -> Domain events
  -> Save aggregate + outbox atomically
  -> Worker claims outbox item
  -> Automation/notification/integration handler
  -> Idempotent result + execution record
```

Do not perform unreliable external side effects before the business transaction commits.

## 9. SLA clock

SLA is modeled as a domain/application subsystem, not a single ExpectedDate column.

An SLA instance records the applied policy/target and computed deadlines. Deadline calculation considers:

- target type (first response, next response, resolution, etc.);
- business calendar;
- tenant time zone;
- holidays;
- pause/resume rules;
- ticket state transitions.

The system must retain enough history to explain why a deadline was calculated or breached.

## 10. Routing

Routing decisions must be explainable.

Each automated routing decision should record inputs such as:

- eligible team/agents;
- priority;
- required skills;
- capacity/load;
- strategy;
- fallback reason;
- selected assignee.

Start with simple deterministic strategies before advanced optimization.

## 11. Automation engine

Automation follows:

```text
Trigger -> Conditions -> Actions
```

Triggers may be domain events or scheduled checks. Conditions are pure/evaluable against an explicit context. Actions invoke application commands/ports rather than mutate database rows directly.

Automation executions require:

- rule version/id;
- triggering event/job id;
- idempotency key;
- start/end timestamps;
- result/status;
- failure details;
- retry count.

## 12. Security boundaries

Authorization is checked at use-case boundaries and reinforced for resource ownership/tenant scope.

Rich text must be sanitized before trusted rendering. Attachment downloads require authorization and cannot rely on obscurity of a URL. Secrets never live in source control.

## 13. Testing strategy

### Domain tests

Fast tests for aggregate invariants, transitions, priority calculation, SLA calculations, and policies.

### Application tests

Use-case behavior with ports faked or replaced by focused test doubles.

### Integration tests

Real PostgreSQL/Redis/storage adapters through disposable containers where appropriate.

### API functional tests

Authentication/authorization, validation, problem details, concurrency, tenant isolation, and critical journeys.

### Architecture tests

Enforce dependency rules and prevent infrastructure/framework references from leaking into Domain/Application.

### Web E2E

Playwright covers critical requester/agent/admin journeys.

## 14. Observability

Use structured logs, traces, and metrics with correlation/trace ids.

Critical operations to instrument include:

- ticket commands;
- SLA calculation/breach jobs;
- routing decisions;
- automation execution;
- outbox processing;
- notification delivery;
- inbound integrations.

## 15. Evolution rule

Do not extract a microservice because a module looks independent on a diagram. Extraction requires a concrete operational reason such as independent scaling, isolation, ownership, deployment cadence, or reliability requirements that outweigh distributed-system complexity.
