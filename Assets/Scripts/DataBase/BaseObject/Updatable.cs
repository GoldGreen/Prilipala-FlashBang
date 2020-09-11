using UnityEngine.Events;

public abstract class Updatable<T> : IUpdatable
where T : Updatable<T>
{
    public UnityEvent<T> OnDataChanged { get; } = new UnityEvent<T>();

    public void Update()
    {
        OnDataChanged.Invoke((T)this);
    }
}
