using UnityEngine;

public class Heel : MonoBehaviour, ISetData<EquipData>, IChangeCharacter
{
    private ArmorEntity armorEntity = new ArmorEntity();

    public virtual void SetData(EquipData equipData)
    {
        equipData.SetTo(armorEntity);
    }

    public virtual void Change(PlayerCharacterLogic character)
    {
        armorEntity.Change(character);
    }
}