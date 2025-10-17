public interface IDataCard<T>
{
    public void Initialize(T data);
    public void SetData();
}
public interface IDataCard
{
    public void Initialize();
    public void SetData();
}