using System.Collections.Generic;

public interface IDataBase
{
    Money Money { get; }
    Score Score { get; }
    Character Character { get; }
    T Find<T>(IdCode idCode) where T : BaseObjectData<T>;
    IEnumerable<EquipData> Equips { get; }
    IEnumerable<InteractiveData> Interactives { get; }
}