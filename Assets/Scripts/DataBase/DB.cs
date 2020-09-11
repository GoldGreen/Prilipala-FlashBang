using System.Linq;

public static class DB
{
    public static IDataBase Data { get; set; }

    static DB()
    {
        Data = DataBase.GetElement();
    }

    public static bool PaidOpen(this BaseObjectData objectData)
    {
        if (!objectData.IsOpened)
        {
            if (objectData is EquipData && Data.Money.TakeEquipMoney(objectData.OpenCost) ||
                objectData is InteractiveData && Data.Money.TakeInteractiveMoney(objectData.OpenCost))
            {
                objectData.Open();
                return true;
            }
        }
        return false;
    }

    public static bool PaidIncreasing(this BaseObjectData objectData)
    {
        if (objectData.IsOpened && objectData.Level < objectData.MaxLevel)
        {
            if (Data.Money.TakeLevelMoney(objectData.IncreasingCost))
            {
                objectData.IncreaseLevel();
                return true;
            }
        }
        return false;
    }

    public static bool PaidObjectIncreasing(this BaseObjectData objectData)
    {
        if (objectData.Level == objectData.MaxLevel && objectData.ObjectLevel < ObjectLevel.platinum)
        {
            if (objectData is EquipData && Data.Money.TakeEquipMoney(objectData.IncreasingObjectCost) ||
                objectData is InteractiveData && Data.Money.TakeInteractiveMoney(objectData.IncreasingObjectCost))
            {
                objectData.IncreaseObjectLevel();
                return true;
            }
        }
        return false;
    }

    public static void ChangeSpecialSelection(this BaseObjectData objectData, bool newSelection)
    {
        objectData.ChangeSelection(newSelection);

        if (objectData.IsSelected && objectData is EquipData)
        {
            var equipData = objectData as EquipData;
            Data.Equips
                .Select(x => x as EquipData)
                .Where(x => x != equipData && x.TypeOfEquip == equipData.TypeOfEquip)
                .ForEach(x => x.ChangeSelection(false));
        }
    }

    public static void ReverseSelection(this BaseObjectData objectData)
    => objectData.ChangeSpecialSelection(!objectData.IsSelected);
}