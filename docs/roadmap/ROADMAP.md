# V2 Delivery Roadmap

This roadmap is intentionally ordered by dependency and business value. A later phase may begin only when its required foundation is stable enough to avoid creating a second source of truth.

## Phase 0 — Public repository safety and baseline

- [x] Create isolated migration branch.
- [x] Remove committed active JWE secret from runtime configuration.
- [x] Remove machine-specific active connection string from runtime configuration.
- [x] Remove committed demo credential note.
- [x] Make encryption configuration fail fast when a valid key is not supplied securely.
- [x] Add local secret/upload ignore rules.
- [x] Add repository security policy.
- [x] Add public-facing README and architecture direction.
- [ ] Complete repository-wide secret-pattern scan.
- [ ] Review Git history for credentials requiring rotation/history remediation.
- [ ] Review dependencies and current V1 build warnings/errors.
- [ ] Add automated secret/dependency scanning to CI.

## Phase 1 — Platform foundation

- [ ] Pin .NET 10 LTS SDK policy.
- [ ] Create V2 solution boundary under `src/` and `tests/`.
- [ ] Add Domain, Application, Infrastructure, API, Worker, AppHost, and ServiceDefaults projects.
- [ ] Introduce PostgreSQL development runtime.
- [ ] Introduce Aspire orchestration and OpenTelemetry baseline.
- [ ] Add Docker-based development path.
- [ ] Create Angular 22 client workspace.
- [ ] Establish API problem-details, validation, versioning, and OpenAPI conventions.
- [ ] Add architecture tests for dependency rules.
- [ ] Establish CI build/test/lint pipeline.

### Exit criteria

- V2 solution builds cleanly.
- API health endpoint runs through local orchestration.
- Angular shell runs and can call the API.
- database is reachable through configured development environment.
- architecture tests protect Domain/Application boundaries.
- no active secrets are required in tracked files.

## Phase 2 — Identity, workspace, and authorization

- Workspace/tenant aggregate and membership.
- User/agent/requester identity mapping.
- Role and permission model.
- Authentication/session model.
- Current-workspace resolution.
- Tenant-scoped authorization policies.
- Cross-tenant isolation tests.
- Security audit records for privileged actions.

### Exit criteria

A user can authenticate, select/access only authorized workspaces, and every tenant-owned API operation is isolation-tested.

## Phase 3 — Ticket core

- Ticket aggregate and immutable identity.
- Canonical lifecycle.
- requester and organization references.
- category/request type.
- priority model.
- team/assignee.
- tags and followers.
- resolution/closure/reopen behavior.
- related/duplicate ticket relationships.
- optimistic concurrency.
- complete ticket audit/timeline facts.

### Exit criteria

Requester and agent critical journeys are functional through API and web UI without relying on legacy V1 services.

## Phase 4 — Conversations and attachments

- public replies;
- internal notes;
- mentions;
- system timeline entries;
- attachment metadata/storage abstraction;
- secure download authorization;
- file-size/type policy;
- rich-text sanitization;
- notification intents for conversation events.

## Phase 5 — Queue engine

- queue definition;
- filter model;
- ordering;
- ownership/visibility;
- My Tickets;
- Unassigned;
- Team queues;
- priority queues;
- saved views;
- SLA-risk-compatible sorting contract.

## Phase 6 — SLA engine

- SLA policy/targets;
- business calendars;
- holidays;
- tenant time zone;
- first-response and resolution clocks;
- pause/resume policies;
- at-risk and breached states;
- durable breach/escalation events;
- explainable deadline history.

## Phase 7 — Routing

Implement progressively:

1. manual assignment;
2. team assignment;
3. round robin;
4. least loaded;
5. skill-based routing;
6. capacity-aware routing;
7. priority/SLA-risk-aware routing;
8. fallback and escalation.

Every automated routing decision must be explainable and auditable.

## Phase 8 — Automation engine

- trigger definitions;
- conditions;
- actions;
- domain-event triggers;
- scheduled triggers;
- rule enable/disable/versioning;
- durable execution log;
- idempotency;
- retries/dead-letter handling;
- outbox-driven processing.

## Phase 9 — Customer portal and service catalog

- requester portal;
- service catalog categories/items;
- request types;
- dynamic forms;
- custom fields/options;
- conditional field visibility;
- organization-specific request availability;
- requester ticket history and reply flow.

## Phase 10 — Knowledge and self-service

- article lifecycle;
- categories;
- permissions;
- search;
- ticket/article linking;
- resolution-to-draft workflow;
- suggested articles before ticket submission;
- deflection analytics.

## Phase 11 — Approvals and configurable workflows

- approval requests;
- individual/group approvers;
- any/all approval policies;
- sequential/parallel stages;
- reminders and expiry;
- reusable workflow definitions;
- custom states/transitions constrained by canonical lifecycle semantics.

## Phase 12 — ITSM capabilities

- incidents;
- major incidents;
- problems and root-cause analysis;
- change requests;
- change risk/approval;
- service relationships;
- incident/problem/change linking.

## Phase 13 — Integrations and channels

- outbound email;
- inbound email-to-ticket/reply;
- webhooks;
- API credentials/integration identities;
- external connector framework;
- Slack/Teams adapters when justified;
- delivery logs and replay safety.

## Phase 14 — Analytics and operations

- ticket volume/backlog;
- new vs resolved;
- response/resolution time;
- SLA attainment/breach;
- age distribution;
- reopen/escalation rate;
- agent/team workload;
- organization/service trends;
- CSAT;
- operational health for workers/outbox/notifications.

## Phase 15 — AI assistance

AI is advisory, not an authoritative business source of truth.

Candidate capabilities:

- ticket summarization;
- suggested category/priority;
- duplicate detection;
- knowledge recommendations;
- reply drafting;
- conversation summarization at handoff;
- automation-rule authoring assistance.

Every AI-derived state-changing action requires explicit policy, confidence handling, and auditability.

## Phase 16 — Production hardening

- rate limiting;
- load/concurrency testing;
- accessibility audit;
- security review;
- backup/restore procedure;
- retention policy;
- failure/retry testing;
- observability dashboards;
- deployment runbooks;
- upgrade/migration strategy.

## Phase 17 — Open-source release

- selected license;
- CONTRIBUTING.md;
- CODE_OF_CONDUCT.md;
- issue/PR templates;
- architecture diagrams;
- screenshots/demo flow;
- seeded demo data;
- one-command local startup path;
- verified clean-install documentation;
- changelog;
- tagged release.

## Definition of done for the V2 foundation

The project is not considered production-ready because it has many features. It is production-ready only when critical business journeys are secure, observable, tenant-isolated, documented, tested at the appropriate layers, and reproducible from a clean environment.
