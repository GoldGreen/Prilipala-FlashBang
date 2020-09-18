using System;
using UnityEngine;

public class Mushroom : MonoBehaviour, IHave<IdCode>, ISetData<InteractiveData>, IInteractWithPhysics, IInteractWithPlayerCharacter, IInteract, IDisposable, ILinkWithShower
{
    public IdCode Item => IdCode.Mushroom;
    [SerializeField] private DamageEntity damageEntity;

    [SerializeField] private float multiplyScale;
    [SerializeField] private float maxScale = 1.1f;

    private float damagedTime = 0.25f;
    private int damagingCount = 10;

    [SerializeField] private Sprite effectIcon;

    public EffectShower EffectShower { get; set; }

    public void SetData(InteractiveData interactiveData)
    {
        interactiveData.SetTo(damageEntity);

        damagedTime = interactiveData.DamageTime;
        damagingCount = interactiveData.DamageCount;
    }

    public void Interact(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    {
        if (playerTransform.localScale.x < maxScale)
        {
            playerTransform.localScale *= multiplyScale;
        }
    }

    public void Interact(PlayerCharacterLogic playerCharacter)
    {
        damageEntity.CountAttack(playerCharacter);
    }

    public void Interact()
    {
        EffectShower.AddOrUpdate(effectIcon, EffectType.Single, damagingCount * damagedTime);
        Dispose();
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
    }
}