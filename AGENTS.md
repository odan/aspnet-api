# Agent instructions

## Layout

- `Application/` — use cases as vertical slices. Keep handler, validation, data access, and request/response types in one folder.
- `Application/Common/` — shared application helpers.
- `Infrastructure/` — technical adapters (persistence, security, logging, localization, exception mapping).
- `Program.cs` — composition root only: configuration, DI, middleware, routes.

## Conventions

- Extend an existing slice or infrastructure area. Do not add a new layer or project.
- Register services in the existing DI extensions. Do not scatter `Add*` calls in `Program.cs`.
- Inject dependencies as parameters. Do not use a service locator.
- No `Async` suffix on method names. Keep cancellation tokens where supported.
- Named, block-bodied methods only. No lambdas or expression-bodied class methods for handlers and repositories.

## Handlers

- Orchestrate the use case and return `IResult`. No routing, no SQL.
- Map routes in `Program.cs` under the authorized `/api` group so they inherit the API-key policy. Add `.WithTags(...)` and summaries for OpenAPI.
- Validate before database work.
- Throw application exceptions for validation and expected failures. Map them to HTTP in Infrastructure. Log unexpected exceptions and rethrow; do not expose exception details.

## Repositories

- Own data access. Inject only `QueryFactory`. Do not create connections, inject connection strings, or use HTTP types.
- Use SqlKata with parameterized values. Never concatenate request data into SQL.
- Accept `IDbTransaction` when the method may join a unit of work, and pass it to every SqlKata call.

## Transactions

- The handler defines the boundary when multiple repository operations must succeed or fail together. Skip a transaction for a standalone operation.
- Begin with `ITransactionFactory`. Pass the `IDbTransaction` to repositories. Commit or roll back through the transaction API; dispose to roll back. No manual transaction SQL.

## Schema

- Forward-only SQL files in `Infrastructure/Migrations/`. Keep the full current schema in `Infrastructure/Schema/schema.sql`.
- Do not change schema in startup or repositories.

## Secrets and verification

- Never read `.env`.
- Run `dotnet build` after code or project changes.
- Do not add tests unless asked.
