using UnityEngine;

public class Explosion : MonoBehaviour, ISetData<InteractiveData>, IInteractWithPlayerCharacter, IRepoolable
{
    [SerializeField] private DamageEntity damageEntity;
    [SerializeField] float damagedTime = 0.5f;

    private ScaleAnimation scaleAnimation;

    private void Awake()
    {
        scaleAnimation = GetComponent<ScaleAnimation>();
        scaleAnimation.Scale = transform.localScale;
    }

    public void SetData(InteractiveData interactiveData)
    {
        interactiveData.SetTo(damageEntity);
    }

    public void Interact(PlayerCharacterLogic playerCharacter)
    {
        damageEntity.Attack(playerCharacter);
    }

    public void Repool()
    {
        scaleAnimation.Show();
        StartCoroutine(CoroutineT.Single
        (
            () => gameObject.SetActive(false),
            damagedTime
        ));
    }
}