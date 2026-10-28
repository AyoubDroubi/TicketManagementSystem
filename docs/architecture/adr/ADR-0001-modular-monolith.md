# ADR-0001: Start V2 as an API-first modular monolith

- Status: Accepted
- Date: 2026-08-13

## Context

The legacy application combines UI composition, Identity, EF Core, repositories, and business services in a small solution. V2 needs substantially richer behavior: ticket lifecycle rules, queues, SLA clocks, routing, automation, organizations, approvals, knowledge, ITSM, integrations, and analytics.

Splitting those capabilities into networked services at the start would add distributed transactions, message consistency, deployment coordination, service discovery, tracing, retry semantics, and operational overhead before the product has evidence that those costs are justified.

At the same time, retaining an undifferentiated layered monolith would make module ownership and business boundaries increasingly unclear.

## Decision

V2 will be implemented as an API-first modular monolith.

The initial physical projects are:

```text
Ticketing.Domain
Ticketing.Application
Ticketing.Infrastructure
Ticketing.Api
```

Business modules remain explicit within those projects and follow the dependency rule documented in `ARCHITECTURE.md`.

The web client communicates through the API rather than referencing server assemblies.

Durable internal side effects that may later cross process/service boundaries use domain events plus an outbox rather than direct in-transaction network calls.

## Consequences

### Positive

- one deployment unit while the product is evolving quickly;
- straightforward transactional consistency;
- lower local-development and operational cost;
- enforceable business module boundaries;
- API-first clients and integrations;
- future extraction remains possible because boundaries and events are explicit.

### Negative

- module boundaries require discipline and architecture tests;
- a single deployment unit cannot independently scale one module;
- poor code organization could still devolve into a tightly coupled monolith if dependency rules are ignored.

## Extraction rule

A module may be considered for extraction only when there is concrete evidence such as independent scaling, reliability isolation, ownership, regulatory isolation, or deployment-cadence requirements that outweigh distributed-system complexity.
