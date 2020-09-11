using UnityEngine;
using UnityEngine.Events;

public abstract class BaseObjectData : IUpdatable
{
    public bool IsOpened
    {
        get => DynamicParatetrs.IsOpened;
        private set => DynamicParatetrs.IsOpened = value;
    }

    public bool IsSelected
    {
        get => DynamicParatetrs.IsSelected;
        private set => DynamicParatetrs.IsSelected = value;
    }

    public int Level
    {
        get => DynamicParatetrs.Level;
        private set => DynamicParatetrs.Level = value;
    }

    public ObjectLevel ObjectLevel
    {
        get => DynamicParatetrs.ObjectLevel;
        private set => DynamicParatetrs.ObjectLevel = value;
    }

    protected float FullImpact => (float)ObjectLevel * MaxLevel + Level;
    protected float ObjectLevelImpact => (float)ObjectLevel;

    private float IncreasingImpact => Mathf.Pow(1.05f, FullImpact);
    private float IncreasingObjectImpact => Mathf.Pow(4.5f, ObjectLevelImpact);

    public DynamicParatetrs DynamicParatetrs { get; }

    public string Name { get; }
    public long OpenCost { get; }
    public int MaxLevel { get; }

    public long IncreasingObjectCost => (long)(increasingObjectCost * IncreasingObjectImpact);
    private readonly long increasingObjectCost;

    public long IncreasingCost => (long)(increasingCost * IncreasingImpact);
    private readonly long increasingCost;

    public UnityEvent<BaseObjectData> OnDataChanged { get; private set; } = new UnityEvent<BaseObjectData>();

    protected BaseObjectData(DynamicParatetrs dynamicParatetrs, string name, long openCost, int maxLevel, long increasingObjectCost, long increasingCost)
    {
        DynamicParatetrs = dynamicParatetrs;
        Name = name;
        OpenCost = openCost;
        MaxLevel = maxLevel;
        this.increasingObjectCost = increasingObjectCost;
        this.increasingCost = increasingCost;
    }

    public void Open()
    {
        if (!IsOpened)
        {
            IsOpened = true;
            OnDataChanged.Invoke(this);
        }
    }

    public void ChangeSelection(bool newSelection)
    {
        if (IsOpened && IsSelected != newSelection)
        {
            IsSelected = newSelection;
            OnDataChanged.Invoke(this);
        }
    }

    public void IncreaseLevel()
    {
        if (IsOpened)
        {
            Level++;
            OnDataChanged.Invoke(this);
        }
    }

    public void IncreaseObjectLevel()
    {
        if (IsOpened && Level == MaxLevel && ObjectLevel < ObjectLevel.platinum)
        {
            ObjectLevel++;
            Level = 0;
            OnDataChanged.Invoke(this);
        }
    }

    public void Update() => OnDataChanged.Invoke(this);
}