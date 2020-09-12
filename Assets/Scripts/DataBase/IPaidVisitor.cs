public interface IPaidOpenVisitor
{
    bool Visit(InteractiveData interactiveData);
    bool Visit(EquipData equipData);
}

public interface IPaidOpenVisited
{
    bool Accemp(IPaidOpenVisitor visitor);
}