namespace MyApi.Infrastructure;

public interface ITransaction
{
    public void Begin();
    public void Commit();
    public void Rollback();

}