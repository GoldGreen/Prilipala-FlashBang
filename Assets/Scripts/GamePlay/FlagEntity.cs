using UnityEngine;
using UnityEngine.Events;

public class FlagEntity
{
    private int state = 0;
    public UnityEvent OnFlagBeingTrue { get; } = new UnityEvent();

    public void DenyAt(MonoBehaviour coroutineSource, float time, UnityAction denyActions = null)
    {
        state++;
        CoroutineT.Single(() =>
        {
            RemoveState();
            denyActions?.Invoke();
        }, time).Start(coroutineSource);
    }

    private void RemoveState()
    {
        if (state > 0)
        {
            state--;

            if (state == 0)
                OnFlagBeingTrue.Invoke();
        }
    }

    public void Update()
    {
        state = 0;
        OnFlagBeingTrue.Invoke();
    }

    public static implicit operator bool(FlagEntity flagEntity)
    {
        return flagEntity.state == 0;
    }
}