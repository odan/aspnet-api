namespace MyApi.Infrastructure.Persistence.Transactions;

using Microsoft.EntityFrameworkCore.Storage;

public sealed class EfTransaction : ITransaction, IDisposable, IAsyncDisposable
{
    private readonly AppDbContext _db;

    private IDbContextTransaction? _tx;

    public EfTransaction(AppDbContext db)
    {
        _db = db;
    }

    public void Begin()
    {
        // Starts a database transaction for the current DbContext connection
        _tx = _db.Database.BeginTransaction();
    }

    public void Commit()
    {
        if (_tx is null)
            throw new InvalidOperationException("Transaction has not been started.");

        _db.SaveChanges();
        _tx.Commit();
    }

    public void Rollback()
    {
        if (_tx is null)
            return;

        _tx.Rollback();
    }

    public void Dispose()
    {
        _tx?.Dispose();
        _tx = null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_tx is not null)
        {
            await _tx.DisposeAsync();
            _tx = null;
        }
    }
}