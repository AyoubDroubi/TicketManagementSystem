# ADR-0002: Preserve V1 as a behavior reference and replace it incrementally

- Status: Accepted
- Date: 2026-08-13

## Context

The repository already contains useful ticketing behavior in the .NET 8/Blazor application. Rewriting without reading that behavior risks losing requirements. Directly extending the V1 architecture, however, would carry framework coupling, mutable domain entities, UI/service orchestration, synchronous persistence calls, string workflow state, and legacy configuration practices into V2.

## Decision

V1 remains in place temporarily as a behavior reference while V2 is built under `src/`.

For each capability:

1. inventory the V1 user-visible/business behavior;
2. define the V2 domain/application contract;
3. implement and test one canonical V2 path;
4. migrate the UI/integration path to V2;
5. remove the superseded V1 implementation when no supported journey depends on it.

V2 must not call V1 repositories/services as a shortcut. Shared code may be moved only after it has been reviewed against V2 dependency rules.

## Consequences

- migration can be incremental and reviewable;
- existing business intent remains inspectable;
- temporary duplication exists at the repository level during migration;
- documentation must clearly identify the canonical implementation;
- legacy code is removed after migration rather than retained indefinitely as V1/V2/V3 variants.
