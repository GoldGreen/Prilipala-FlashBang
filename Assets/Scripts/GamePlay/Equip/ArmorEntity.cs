public class ArmorEntity : ISetData<EquipData>, IChangeCharacter
{
    private float physArmor;
    private float magicArmor;
    private float electricArmor;

    public void SetData(EquipData equip)
    {
        physArmor = equip.AddedPhysArmor;
        magicArmor = equip.AddedMagicArmor;
        electricArmor = equip.AddedElectricArmor;
    }

    public void Change(PlayerCharacterLogic character)
    {
        character.AddArmor(physArmor, magicArmor, electricArmor);
    }
}