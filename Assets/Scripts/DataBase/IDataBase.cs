using System.Collections.Generic;

public interface IDataBase : ISelectVisitor, IPaidIncreaseObjectVisitor, IPaidOpenVisitor
{
    Money Money { get; }
    Score Score { get; }
    Character Character { get; }
    T Find<T>(IdCode idCode) where T : BaseObjectData<T>;
    Achievment Find(AchievmentCode code);
    bool PaidIncrease<T>(BaseObjectData<T> baseObjectData) where T : BaseObjectData<T>;
    IEnumerable<EquipData> Equips { get; }
    IEnumerable<InteractiveData> Interactives { get; }
}