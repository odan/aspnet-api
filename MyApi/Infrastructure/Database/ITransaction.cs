namespace MyApi.Infrastructure.Database;

public interface ITransaction
{
    public void Begin();
    public void Commit();
    public void Rollback();

}