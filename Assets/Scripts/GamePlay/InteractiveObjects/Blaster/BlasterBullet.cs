using UnityEngine;

public class BlasterBullet : MonoBehaviour, ISetData<InteractiveData>, IInteractWithPlayerCharacter, IInteract, IDestroyedByWave, IInteractWithPhysics
{
    [SerializeField] DamageEntity damageEntity;
    [SerializeField] ParticleSystem hittingEffect;

    private Transform hittingEffectTransform;
    private new Transform transform;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        hittingEffectTransform = hittingEffect.transform;
        hittingEffectTransform.SetParent(null);
    }

    public void SetData(InteractiveData interactiveData)
    {
        interactiveData.SetTo(damageEntity);
    }

    public void Interact(PlayerCharacterLogic playerCharacter)
    {
        damageEntity.Attack(playerCharacter);
    }

    public void Interact(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    {
        hittingEffect.transform.position = transform.position;
        hittingEffect.Play();
    }
    public void Interact()
    {
        gameObject.SetActive(false);
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}