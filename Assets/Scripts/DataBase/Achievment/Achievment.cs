public enum AchievmentCode
{
    plungerAchievment
}

public class Achievment : Updatable<Achievment>
{
    public AchievmentProgress AchievmentProgress { get; }
    public int MaxValue { get; }
    public string Name { get; }

    public int ProgressValue
    {
        get => AchievmentProgress.Value;
        set
        {
            AchievmentProgress.Value = value;
            Update();
        }
    }
    public float NormalProgressValue => AchievmentProgress.NormalProgress(MaxValue);

    public Achievment(AchievmentProgress achievmentProgress, string name, int maxValue)
    {
        AchievmentProgress = achievmentProgress;
        Name = name;
        MaxValue = maxValue;
    }
}
