namespace MyApi.Infrastructure.Persistence.Transactions;

public interface ITransaction
{
    public void Begin();
    public void Commit();
    public void Rollback();

}