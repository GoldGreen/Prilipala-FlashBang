using UnityEngine;

public class PlungerBullet : MonoBehaviour, ISetData<InteractiveData>, IInteractWithPlayerCharacter, IInteract, IDestroyedByWave
{
    [SerializeField] private DamageEntity damageEntity;

    public void SetData(InteractiveData interactiveData)
    {
        damageEntity.SetData(interactiveData);
    }

    public void Interact(PlayerCharacterLogic playerCharacter)
    {
        damageEntity.Attack(playerCharacter);
    }

    public void Interact()
    {
        Destroy();
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}