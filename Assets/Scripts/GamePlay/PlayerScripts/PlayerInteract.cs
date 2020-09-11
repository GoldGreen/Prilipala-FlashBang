using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private new Transform transform;
    private new Rigidbody2D rigidbody2D;
    private PlayerControl playerControl;
    private PlayerCharacterLogic playerHealth;
    private EquipWearing equipWearing;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerControl = GetComponent<PlayerControl>();
        playerHealth = GetComponent<PlayerCharacterLogic>();

        equipWearing = GetComponent<EquipWearing>();
    }

    private void Start()
    {
        playerHealth.GlobalUpdateHealth();

        foreach (var equip in equipWearing.GetPreparedEquips())
        {
            equip.transform.SetParent(transform, false);
            equip.GetComponent<IChangeCharacter>()?.Change(playerHealth);
            equip.GetComponent<IChangePhysics>()?.Change(transform, rigidbody2D, playerControl);
            equip.GetComponent<IChange>()?.Change();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        col.GetComponent<IInteractWithPlayerCharacter>()?.Interact(playerHealth);
        col.GetComponent<IInteractWithPhysics>()?.Interact(transform, rigidbody2D, playerControl);
        col.GetComponent<IInteract>()?.Interact();
    }
}