using UnityEngine;

public class Arrow : MonoBehaviour, ISetData<InteractiveData>, IInteractWithPlayerCharacter, IInteract, IDestroyedByWave
{
    [SerializeField] private DamageEntity damageEntity;

    public bool IsRotating { get; set; } = false;

    private Rigidbody2D rigitbody;
    private new Transform transform;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        rigitbody = GetComponent<Rigidbody2D>();
    }

    public void SetData(InteractiveData interactiveData) => interactiveData.SetTo(damageEntity);

    private void FixedUpdate()
    {
        if (IsRotating)
            transform.rotation = rigitbody.velocity.ToQuartetion();
    }

    public void Interact(PlayerCharacterLogic playerCharacter) => damageEntity.Attack(playerCharacter);
    public void Interact() => Destroy();
    public void Destroy() => gameObject.SetActive(false);
}
