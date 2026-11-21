using Npgsql;
using System.Data.Common;

namespace Demo.Api.Infrastructure.Persistence;

public sealed class NpgsqlTransactionFactory(NpgsqlConnection connection) : ITransactionFactory
{
    public async Task<DbTransaction> BeginTransaction(CancellationToken ct = default)
    {
        await connection.OpenAsync(ct);
        return await connection.BeginTransactionAsync(ct);
    }
}