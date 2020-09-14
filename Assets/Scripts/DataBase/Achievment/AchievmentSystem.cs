using System;
using UnityEngine.Events;

public static class AchievmentSystem
{
    public static IDisposable Subscribe(this AchievmentCode achievmentCode, UnityAction<Achievment> handler)
    {
        return DB.Data.Find(achievmentCode).OnDataChanged.Subscribe(handler);
    }

    public static UnityAction GetIncreasedHandler(this AchievmentCode achievmentCode, int increaseCount = 1)
    {
        var achievement = DB.Data.Find(achievmentCode);
        return () => achievement.ProgressValue += increaseCount;
    }
}
