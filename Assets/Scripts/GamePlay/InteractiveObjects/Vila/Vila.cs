using System;
using UnityEngine;

public class Vila : MonoBehaviour, IHaveIdCode, ISetData<InteractiveData>, IInteractWithPlayerCharacter, IDisposable, IRepoolable
{
    public IdCode IdCode => IdCode.Vila;

    [SerializeField] private DamageEntity damageEntity;

    private MovingObject movingObject;

    private void Awake()
    {
        movingObject = GetComponent<MovingObject>();
    }

    public void SetData(InteractiveData interactiveData)
    {
        damageEntity.SetData(interactiveData);
    }

    private void Shoot()
    {
        movingObject.Speed = new Vector3(20, 0, 0);
        movingObject.UpdateSpeedAndDelta();
    }

    public void Interact(PlayerCharacterLogic playerCharacter)
    {
        damageEntity.Attack(playerCharacter);
    }

    public void Repool()
    {
        StopAllCoroutines();
        StartCoroutine(CoroutineT.Single(Shoot, 2));
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
    }
}
