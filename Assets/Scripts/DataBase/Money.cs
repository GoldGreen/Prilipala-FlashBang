using System;
using UnityEngine;

[Serializable]
public class Money : Updatable<Money>
{
    public long Interactive => interactive;
    [SerializeField] private long interactive;

    public long Equip => equip;
    [SerializeField] private long equip;

    public long Level => level;
    [SerializeField] private long level;

    public Money(Money obj)
    {
        interactive = obj.interactive;
        equip = obj.equip;
        level = obj.level;
    }

    public Money()
    {
        interactive = 199999999997;
        equip = 199999999997;
        level = 29999999999997;
    }

    public bool TakeInteractiveMoney(long money)
    {
        if (interactive < money)
        {
            return false;
        }

        interactive -= money;
        Update();
        return true;
    }

    public bool TakeEquipMoney(long money)
    {
        if (equip < money)
        {
            return false;
        }

        equip -= money;
        Update();
        return true;
    }

    public bool TakeLevelMoney(long money)
    {
        if (level < money)
        {
            return false;
        }

        level -= money;
        Update();
        return true;
    }

    public void AddMoney(long levelMoney, long interactiveMoney, long equipMoney)
    {
        level += levelMoney;
        interactive += interactiveMoney;
        equip += equipMoney;
        Update();
    }
}