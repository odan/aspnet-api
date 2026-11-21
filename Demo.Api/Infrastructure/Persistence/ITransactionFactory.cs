using System.Data.Common;

namespace Demo.Api.Infrastructure.Persistence;

public interface ITransactionFactory
{
    Task<DbTransaction> BeginTransaction(CancellationToken ct = default);
}