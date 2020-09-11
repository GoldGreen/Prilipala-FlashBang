public interface IPoolable
{
    void Float();
    void Drown();
}

public interface IPoolable<T>
{
    void Float(T arg);
    void Drown();
}
