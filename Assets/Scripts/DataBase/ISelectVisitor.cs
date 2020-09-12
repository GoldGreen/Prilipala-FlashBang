public interface ISelectVisitor
{
    void Visit(InteractiveData interactiveData, bool newSelection);
    void Visit(EquipData equipData, bool newSelection);
}

public interface ISelectVisited
{
    void Accemp(ISelectVisitor visitor, bool newSelection);
}
