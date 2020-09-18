using System;
using UnityEngine;

public class Timber : MonoBehaviour, IHave<IdCode>, ISetData<InteractiveData>, IInteractWithPlayerCharacter, IDisposable
{
    public IdCode Item => IdCode.Timber;

    [SerializeField] private DamageEntity damageEntity;

    private int angleIteration = 0;

    private new Transform transform;
    private new Collider2D collider;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        collider = GetComponent<Collider2D>();
    }

    public void SetData(InteractiveData interactiveData)
    {
        interactiveData.SetTo(damageEntity);

        damageEntity.ActionInCoroutineAttack =
        x => x.AddColorBy(damageEntity.DamagedColor, interactiveData.DamageTime);

        damageEntity.Collider = collider;
    }

    private void FixedUpdate()
    {
        RotateToOneDegree();
    }

    private void RotateToOneDegree()
    {
        angleIteration--;

        if (angleIteration < 0)
        {
            angleIteration = 360;
        }

        transform.rotation = Quaternion.Euler(0, transform.position.x > 0 ? 180 : 0, angleIteration);
    }

    public void Interact(PlayerCharacterLogic playerCharacter)
    {
        damageEntity.ColliderAttack(playerCharacter);
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
    }
}
