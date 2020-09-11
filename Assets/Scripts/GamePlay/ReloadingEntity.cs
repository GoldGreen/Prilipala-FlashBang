using System;
using UnityEngine;
using UnityEngine.Events;

public class ReloadingEntity<T> : ReloadingEntity, ISetData<T>
where T : BaseObjectData<T>
{
    private readonly Func<T, float> selectReloadingTime;

    public ReloadingEntity(Func<T, float> selectReloadingTime)
        : base()
    {
        this.selectReloadingTime = selectReloadingTime;
    }

    public void SetData(T data)
    {
        ReloadingTime = selectReloadingTime(data);
    }
}

public class ReloadingEntity
{
    public FlagEntity IsReloaded { get; protected set; } = new FlagEntity();
    public float ReloadingTime { get; protected set; }
    public UnityEvent OnReloaded { get; } = new UnityEvent();

    protected ReloadingEntity()
    {
        ReloadingTime = 0;
    }

    public ReloadingEntity(float reloadingTime)
    {
        ReloadingTime = reloadingTime;
    }

    public void StartReload(MonoBehaviour coroutineSource)
    {
        IsReloaded.DenyAt(coroutineSource, ReloadingTime);
    }

    public void FullReload()
    {
        if (!IsReloaded)
        {
            IsReloaded.Update();
            OnReloaded.Invoke();
        }
    }
}