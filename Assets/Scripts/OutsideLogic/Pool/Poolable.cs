using UnityEngine;
using UnityEngine.Events;

public abstract class BasePoolabe
{
    protected readonly GameObject gameObject;
    protected readonly IRepoolable repoolable;

    public UnityEvent OnDrowned { get; } = new UnityEvent();

    protected BasePoolabe(GameObject gameObject)
    {
        this.gameObject = gameObject;
        repoolable = gameObject.GetComponent<IRepoolable>();
        gameObject.SetActive(false);
    }

    public void Drown()
    {
        gameObject.SetActive(false);
        OnDrowned.Invoke();
    }
}

public class Poolable : BasePoolabe, IPoolable
{
    public UnityEvent OnFloated { get; } = new UnityEvent();

    public Poolable(GameObject gameObject)
    : base(gameObject)
    { }

    public void Float()
    {
        gameObject.SetActive(true);
        OnFloated.Invoke();
        repoolable?.Repool();
    }
}

public class Poolable<T> : BasePoolabe, IPoolable<T>
{
    public UnityEvent<T> OnFloated { get; } = new UnityEvent<T>();

    public Poolable(GameObject gameObject)
    : base(gameObject)
    { }

    public void Float(T arg)
    {
        gameObject.SetActive(true);
        OnFloated.Invoke(arg);
        repoolable?.Repool();
    }
}
