using System.Data.Common;

using Npgsql;

namespace Demo.Infrastructure.Persistence;

public sealed class NpgsqlTransactionFactory(NpgsqlConnection connection) : ITransactionFactory
{
    public async Task<DbTransaction> BeginTransaction(CancellationToken ct = default)
    {
        await connection.OpenAsync(ct);
        return await connection.BeginTransactionAsync(ct);
    }
}