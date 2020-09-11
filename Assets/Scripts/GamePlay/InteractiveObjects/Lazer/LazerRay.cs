using UnityEngine;

public class LazerRay : MonoBehaviour, ISetData<InteractiveData>, IInteractWithPhysics, IInteractWithPlayerCharacter, IInteract, ILinkWithShower, IRepoolable
{
    [SerializeField] private DamageEntity damageEntity;

    private float accelerateMultiply;
    private float accelerateMultiplyingTime;

    [SerializeField] private Sprite effectIcon;

    private ScaleAnimation scaleAnimation;
    private new Collider2D collider;
    private new Transform transform;

    private bool needShowEffect = false;
    public EffectShower EffectShower { get; set; }

    private void Awake()
    {
        transform = GetComponent<Transform>();
        collider = GetComponent<Collider2D>();
        scaleAnimation = GetComponent<ScaleAnimation>();

        scaleAnimation.Scale = transform.localScale;
    }

    public void SetData(InteractiveData interactiveData)
    {
        interactiveData.SetAccelerateMultiplyingTo(out accelerateMultiply, out accelerateMultiplyingTime);

        damageEntity.SetData(interactiveData);
        damageEntity.Collider = collider;
    }

    public void Interact(PlayerCharacterLogic playerCharacter)
    {
        damageEntity.ColliderAttack(playerCharacter);
        playerCharacter.AddColorBy(damageEntity.DamagedColor, accelerateMultiplyingTime);
    }

    public void Interact(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    {
        control.MultiplyAccellerate(accelerateMultiply, accelerateMultiplyingTime);
        needShowEffect = control.CanAccelerateMultiply;
    }

    public void Interact()
    {
        if (needShowEffect)
        {
            EffectShower.AddOrUpdate(effectIcon, EffectType.Single, accelerateMultiplyingTime);
        }
    }

    public void Repool()
    {
        scaleAnimation.Show();
    }
}
