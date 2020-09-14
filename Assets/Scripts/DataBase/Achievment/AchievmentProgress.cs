using System;
using UnityEngine;

[Serializable]
public class AchievmentProgress
{
    public int Value
    {
        get => this.value;
        set => this.value = value;
    }
    [SerializeField] private int value;

    public AchievmentProgress()
    {
        value = default;
    }

    public AchievmentProgress(AchievmentProgress achievment)
    {
        value = achievment.value;
    }

    public float NormalProgress(float maxValue)
    {
        return value / maxValue;
    }
}