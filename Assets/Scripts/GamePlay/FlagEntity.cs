using UnityEngine;

public class FlagEntity
{
    private int state = 0;

    public void DenyAt(MonoBehaviour coroutineSource, float time)
    {
        state++;
        CoroutineT.Single(RemoveState, time).Start(coroutineSource);
    }

    private void RemoveState()
    {
        if (state > 0)
            state--;
    }

    public void Update() => state = 0;

    public static implicit operator bool(FlagEntity flagEntity) => flagEntity.state == 0;
}