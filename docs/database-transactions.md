# Database transactions

## Current setup

`ITransactionFactory` abstracts transaction creation. Its PostgreSQL implementation, `NpgsqlTransactionFactory`, opens the scoped `NpgsqlConnection` and starts an asynchronous `DbTransaction`.

Dependency injection registers `NpgsqlConnection` as scoped. The `IDbConnection` supplied to SqlKata's `QueryFactory` resolves to that same scoped instance. This lets a use case start a transaction and pass it to repository methods while every operation uses one connection.

The transaction factory is infrastructure support; no current application handler uses it yet. The current create-user operation performs one insert, so it relies on the database's normal single-statement transaction behavior.

## When to use a transaction

Use a transaction when one use case performs multiple database operations that must either all succeed or all be rolled back. The handler defines that transaction boundary. Keep SQL construction and execution in repositories.

For a single independent database operation, call the repository without creating an explicit transaction.

## Transaction flow

1. Inject `ITransactionFactory` into the handler.
2. Begin the transaction and use `await using` so it is disposed even if the operation throws.
3. Pass the transaction and cancellation token to every repository operation in the unit of work.
4. Commit only after all required operations succeed.
5. On an expected business failure, roll back before returning the appropriate response. Keep the commit after the `try/catch`, so the catch block only handles failures before commit. Log rollback failures separately so they do not hide the original exception.

```csharp
public static async Task<IResult> Handle(
    ITransactionFactory transactionFactory,
    ExampleRepository repository,
    ILoggerFactory loggerFactory,
    CancellationToken cancellationToken)
{
    var logger = loggerFactory.CreateLogger("ExampleHandler");
    await using var transaction =
        await transactionFactory.BeginTransaction(cancellationToken);

    try
    {
        var firstResult = await repository.FirstOperation(transaction, cancellationToken);
        if (firstResult == 0)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Results.Conflict();
        }

        var secondResult = await repository.SecondOperation(transaction, cancellationToken);
        if (secondResult == 0)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Results.Conflict();
        }

    }
    catch (Exception exception)
    {
        logger.LogError(exception, "The example operation failed.");

        try
        {
            await transaction.RollbackAsync(CancellationToken.None);
        }
        catch (Exception rollbackException)
        {
            logger.LogError(rollbackException, "Rolling back the example transaction failed.");
        }

        return Results.Problem("The operation could not be completed.");
    }

    await transaction.CommitAsync(cancellationToken);
    return Results.Ok();
}
```

Repository methods that participate in the transaction accept `IDbTransaction` and pass it to every SqlKata execution call:

```csharp
public Task<int> UpdateStatus(
    int id,
    string status,
    IDbTransaction transaction,
    CancellationToken cancellationToken)
{
    return _db.Query("items")
        .Where("id", id)
        .UpdateAsync(
            new { status },
            transaction: transaction,
            cancellationToken: cancellationToken);
}
```

Do not open a separate connection inside a repository or transaction participant. A separate connection would not be enlisted in this transaction.
