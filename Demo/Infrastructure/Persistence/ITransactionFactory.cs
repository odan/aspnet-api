using System.Data.Common;

namespace Demo.Infrastructure.Persistence;

public interface ITransactionFactory
{
    Task<DbTransaction> BeginTransaction(CancellationToken ct = default);
}