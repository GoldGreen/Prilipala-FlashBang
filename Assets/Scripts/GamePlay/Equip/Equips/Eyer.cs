using UnityEngine;

public class Eyer : MonoBehaviour, ISetData<EquipData>, IChangeCharacter
{
    private float health;

    public virtual void SetData(EquipData equipData) => equipData.SetAddedHealthTo(out health);
    public virtual void Change(PlayerCharacterLogic character) => character.AddHealth(health);
}