using System.Collections.Generic;

public interface IDataBase
{
    Money Money { get; }
    Score Score { get; }
    Character Character { get; }
    BaseObjectData Find(IdCode idCode);
    T Find<T>(IdCode idCode) where T : BaseObjectData;

    IEnumerable<BaseObjectData> Equips { get; }
    IEnumerable<BaseObjectData> Interactives { get; }
}