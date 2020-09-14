using UnityEngine;
using UnityEngine.Events;

public class PlungerBullet : MonoBehaviour, ISetData<InteractiveData>, IInteractWithPlayerCharacter, ISubscribedInteract, IDestroyedByWave
{
    [SerializeField] private DamageEntity damageEntity;

    public UnityEvent OnInteracted => onInteracted;
    [SerializeField] private UnityEvent onInteracted;

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
        onInteracted.Invoke();
        Destroy();
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}