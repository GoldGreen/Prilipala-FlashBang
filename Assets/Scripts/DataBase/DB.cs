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
        return (objectData as IPaidOpenVisited).Accemp(Data);
    }

    public static bool PaidIncreasing<T>(this BaseObjectData<T> objectData)
    where T : BaseObjectData<T>
    {
        return Data.PaidIncrease(objectData);
    }

    public static bool PaidObjectIncreasing<T>(this BaseObjectData<T> objectData)
    where T : BaseObjectData<T>
    {
        return (objectData as IPaidIncreaseObjectVisited).Accemp(Data);
    }

    public static void ChangeSpecialSelection<T>(this BaseObjectData<T> objectData, bool newSelection)
    where T : BaseObjectData<T>
    {
        (objectData as ISelectVisited).Accemp(Data, newSelection);
    }

    public static void ReverseSelection<T>(this BaseObjectData<T> objectData)
    where T : BaseObjectData<T>
    {
        objectData.ChangeSpecialSelection(!objectData.IsSelected);
    }
}