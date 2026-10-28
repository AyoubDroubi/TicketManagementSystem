# Security Policy

## Supported code

The repository is currently undergoing a V2 service-desk architecture migration. Security fixes for the existing V1 application are accepted while the new platform foundation is being built.

## Reporting a vulnerability

Please do not open a public GitHub issue for a suspected vulnerability, exposed credential, or exploit path.

Use GitHub's private vulnerability reporting/security advisory flow when available. Include:

- affected component and version/commit;
- reproduction steps;
- expected and actual behavior;
- impact assessment;
- any suggested mitigation.

## Secret-handling rules

The repository must never contain real credentials, tokens, API keys, private keys, production connection strings, or reusable encryption secrets.

Runtime secrets must be provided through environment variables, .NET user-secrets for local development, or a production secret manager. Example configuration may contain placeholders only.

If a secret is committed, treat it as compromised: remove it from the current tree, rotate/revoke it at the provider, and assess whether repository history needs remediation.

## Security baseline

New V2 code is expected to follow these rules:

- deny by default for authorization-sensitive operations;
- validate tenant/workspace boundaries on every tenant-owned resource;
- validate and sanitize rich-text/HTML input before rendering;
- authorize attachment download independently of attachment discovery;
- use strong cryptographic keys generated outside source control;
- use parameterized data access through supported framework APIs;
- maintain an auditable history for security-sensitive and workflow-changing actions;
- keep dependencies patched and run dependency/secret scanning in CI.
