
namespace MyApi.Database;

public interface ITransaction
{
    public void Begin();
    public void Commit();
    public void Rollback();

}
