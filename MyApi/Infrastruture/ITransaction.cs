namespace MyApi.Infrastruture;

public interface ITransaction
{
    public void Begin();
    public void Commit();
    public void Rollback();

}