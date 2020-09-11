public interface ISetData<T> where T : BaseObjectData<T>
{
    void SetData(T obj);
}