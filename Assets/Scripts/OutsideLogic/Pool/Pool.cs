using System.Collections.Generic;
using System.Linq;

public class Pool
{
    private readonly IPoolable[] poolables;
    public int Length { get; }

    private int index = 0;

    public Pool(IEnumerable<IPoolable> poolables)
    {
        this.poolables = poolables.ToArray();
        Length = this.poolables.Length;
    }

    public void PoolObject()
    {
        poolables[index].Float();
        Next();
    }

    public void HideAll()
    {
        for (int i = 0; i < Length; i++)
        {
            poolables[i].Drown();
        }

        index = 0;
    }

    public void PoolAll()
    {
        for (int i = 0; i < Length; i++)
        {
            PoolObject();
        }
    }

    private void Next() => index = (index + 1) % Length;
}

public class Pool<T>
{
    private readonly IPoolable<T>[] poolables;
    public int Length { get; }

    private int index = 0;

    public Pool(IEnumerable<IPoolable<T>> poolables)
    {
        this.poolables = poolables.ToArray();
        Length = this.poolables.Length;
    }

    public void PoolObject(T arg)
    {
        poolables[index].Float(arg);
        Next();
    }

    public void HideAll()
    {
        for (int i = 0; i < Length; i++)
        {
            poolables[i].Drown();
        }

        index = 0;
    }

    public void PoolAll(T arg)
    {
        for (int i = 0; i < Length; i++)
        {
            PoolObject(arg);
        }
    }

    private void Next() => index = (index + 1) % Length;
}
