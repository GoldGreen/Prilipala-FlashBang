using System;
using UnityEngine;

public enum ObjectLevel
{
    none,
    bronze,
    silver,
    gold,
    platinum
}

[Serializable]
public class DynamicParatetrs
{
    public bool IsOpened
    {
        get => isOpened;
        set => isOpened = value;
    }
    [SerializeField] private bool isOpened;

    public bool IsSelected
    {
        get => isSelected;
        set => isSelected = value;
    }
    [SerializeField] private bool isSelected;

    public int Level
    {
        get => level;
        set => level = value;
    }
    [SerializeField] private int level;

    public ObjectLevel ObjectLevel
    {
        get => objectLevel;
        set => objectLevel = value;
    }
    [SerializeField] private ObjectLevel objectLevel;

    public DynamicParatetrs(DynamicParatetrs obj)
    {
        IsOpened = obj.IsOpened;
        IsSelected = obj.IsSelected;
        Level = obj.Level;
        ObjectLevel = obj.ObjectLevel;
    }

    public DynamicParatetrs()
    {
        IsOpened = default;
        IsSelected = default;
        Level = default;
        ObjectLevel = default;
    }
}