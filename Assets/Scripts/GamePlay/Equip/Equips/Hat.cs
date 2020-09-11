using UnityEngine;

public class Hat : MonoBehaviour, ISetData<EquipData>, IChangeCharacter
{
    private float health;
    private ArmorEntity armorEntity = new ArmorEntity();

    public virtual void SetData(EquipData equipData)
    {
        equipData.SetAddedHealthTo(out health)
        .SetTo(armorEntity);
    }

    public virtual void Change(PlayerCharacterLogic character)
    {
        character.AddHealth(health);
        armorEntity.Change(character);
    }
}