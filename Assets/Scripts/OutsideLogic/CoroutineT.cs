using System;
using System.Collections;
using UnityEngine;

public static class CoroutineT
{
    public static IEnumerator Single(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    public static IEnumerator Infinite(Action action, float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            action();
        }
    }

    public static IEnumerator InfiniteBefore(Action action, float time)
    {
        while (true)
        {
            action();
            yield return new WaitForSeconds(time);
        }
    }

    public static IEnumerator While(Action action, Func<bool> condition, float time, Action endAction = null)
    {
        while (condition())
        {
            yield return new WaitForSeconds(time);
            action();
        }

        endAction?.Invoke();
    }

    public static IEnumerator WhileBefore(Action action, Func<bool> condition, float time, Action endAction = null)
    {
        while (condition())
        {
            action();
            yield return new WaitForSeconds(time);
        }

        endAction?.Invoke();
    }

    public static IEnumerator For(Action action, int count, float time, Action endAction = null)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(time);
            action();
        }

        endAction?.Invoke();
    }

    public static IEnumerator ForBefore(Action action, int count, float time, Action endAction = null)
    {
        for (int i = 0; i < count; i++)
        {
            action();
            yield return new WaitForSeconds(time);
        }

        endAction?.Invoke();
    }

    public static void Start(this IEnumerator corotine, MonoBehaviour source)
    {
        source.StartCoroutine(corotine);
    }
}