using UnityEngine;

public class PowerSocket : MonoBehaviour, IHave<IdCode>, ISetData<InteractiveData>, IInteractWithPlayerCharacter
{
    public IdCode Item => IdCode.PowerSocket;
    [SerializeField] private DamageEntity damageEntity;
    [SerializeField] private GameObject hittingEffectPrefab;
    private ParticleSystem hittingEffect;

    private new Collider2D collider2D;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        hittingEffect = Instantiate(hittingEffectPrefab).GetComponent<ParticleSystem>();
    }

    public void SetData(InteractiveData interactiveData)
    {
        interactiveData.SetTo(damageEntity);
        damageEntity.Collider = collider2D;

        damageEntity.ActionInCoroutineAttack =
        player =>
        {
            hittingEffect.transform.position = player.transform.position;
            hittingEffect.Play();
            player.AddColorBy(damageEntity.DamagedColor, damageEntity.ColoredTime);
        };
    }

    public void Interact(PlayerCharacterLogic playerCharacter)
    {
        damageEntity.ColliderAttack(playerCharacter);
    }
}