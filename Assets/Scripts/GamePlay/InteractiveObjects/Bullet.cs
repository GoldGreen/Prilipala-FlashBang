using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour, ISetData<InteractiveData>, IInteract, ILinkWithShower, IInteractWithPhysics, IInteractWithPlayerCharacter, IDestroyedByWave
{
    [SerializeField] private Sprite hitIcon;
    [SerializeField] private DamageEntity damageEntity;

    private float multiplyAccelerating;
    private float multiplyAcceleratingTime;

    public UnityEvent<Vector2> OnInteract { get; } = new UnityEvent<Vector2>();

    public EffectShower EffectShower { get; set; }

    public void SetData(InteractiveData interactiveData)
    {
        interactiveData.SetTo(damageEntity)
        .SetAccelerateMultiplyingTo(out multiplyAccelerating, out multiplyAcceleratingTime);
        damageEntity.ColoredTime = 0.5f;
    }

    public void Interact()
    {
        gameObject.SetActive(false);
    }

    public void Interact(PlayerCharacterLogic playerCharacter)
    {
        damageEntity.Attack(playerCharacter);

        if (playerCharacter.CanGetDamage)
            OnInteract.Invoke(transform.position);
    }

    public void Interact(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    {
        control.MultiplyAccellerate(multiplyAccelerating, multiplyAcceleratingTime);

        if (control.CanAccelerateMultiply)
            EffectShower.AddOrUpdate(hitIcon, EffectType.Single, multiplyAcceleratingTime);
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}
