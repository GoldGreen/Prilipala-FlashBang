public interface IPaidIncreaseObjectVisitor
{
    bool Visit(InteractiveData interactiveData);
    bool Visit(EquipData equipData);
}

public interface IPaidIncreaseObjectVisited
{
    bool Accemp(IPaidIncreaseObjectVisitor visitor);
}