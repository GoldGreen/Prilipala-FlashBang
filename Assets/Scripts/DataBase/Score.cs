using System;
using UnityEngine;

[Serializable]
public class Score : Updatable<Score>
{
    public long CurrentValue => value;
    [SerializeField] private long value;

    public long MaxValue => maxValue;
    [SerializeField] private long maxValue;

    public long AllValue => allValue;
    [SerializeField] private long allValue;

    public Score(Score obj)
    {
        value = obj.value;
        allValue = obj.allValue;
    }

    public Score()
    {
        value = 0;
        allValue = 0;
        maxValue = 0;
    }

    public void UpdateScore()
    {
        value = 0;
        Update();
    }

    public void AddScore(long count)
    {
        value += count;
        allValue += count;

        if (value > maxValue)
        {
            maxValue = value;
        }

        Update();
    }

    public void ToMoney(out long levelMoney, out long interactiveMoney, out long equipMoney)
    {
        levelMoney = CurrentValue / 3;
        interactiveMoney = CurrentValue / 4;
        equipMoney = CurrentValue / 4;
    }
}