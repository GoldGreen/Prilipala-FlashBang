using System.Linq;

public static class DB
{
    public static IDataBase Data { get; set; }

    static DB()
    {
        Data = DataBase.GetElement();
    }

    public static bool PaidOpen<T>(this BaseObjectData<T> objectData)
    where T : BaseObjectData<T>
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

    public static bool PaidIncreasing<T>(this BaseObjectData<T> objectData)
    where T : BaseObjectData<T>
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

    public static bool PaidObjectIncreasing<T>(this BaseObjectData<T> objectData)
    where T : BaseObjectData<T>
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

    public static void ChangeSpecialSelection<T>(this BaseObjectData<T> objectData, bool newSelection)
    where T : BaseObjectData<T>
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

    public static void ReverseSelection<T>(this BaseObjectData<T> objectData)
    where T : BaseObjectData<T>
    => objectData.ChangeSpecialSelection(!objectData.IsSelected);
}