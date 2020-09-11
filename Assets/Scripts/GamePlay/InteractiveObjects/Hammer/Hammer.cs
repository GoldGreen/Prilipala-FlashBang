using System;
using UnityEngine;

public class Hammer : MonoBehaviour, IHaveIdCode, ISetData<InteractiveData>, IInteractWithPlayerCharacter, IDisposable
{
    private const int UP_DIRECTION = 1;
    private const int DOWN_DIRECTION = -1;

    private const int MIN_ANGLE = 0;
    private const int MAX_ANGLE = 180;

    private const float MIN_ROTATE_SPEED = 1;
    private const float ROTATE_ACCELERATION = 1.0618f;

    public IdCode IdCode => IdCode.Hammer;

    [SerializeField] private DamageEntity damageEntity;

    private float angle = MIN_ANGLE;
    private float rotateSpeed = MIN_ROTATE_SPEED;
    private int rotateDirection = UP_DIRECTION;

    private new Transform transform;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    public void SetData(InteractiveData interactiveData)
    {
        interactiveData.SetTo(damageEntity);
    }

    private void FixedUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(0, transform.position.x > 0 ? 180 : 0, -angle);
        angle += rotateDirection * rotateSpeed;

        if (angle < MIN_ANGLE)
        {
            angle = MIN_ANGLE;
            rotateDirection = UP_DIRECTION;
            rotateSpeed = MIN_ROTATE_SPEED;
            angle += rotateSpeed;
        }
        else if (angle > MAX_ANGLE)
        {
            angle = MAX_ANGLE;
            rotateDirection = DOWN_DIRECTION;
            rotateSpeed = MIN_ROTATE_SPEED;
            angle -= rotateSpeed;
        }

        rotateSpeed *= ROTATE_ACCELERATION;
    }

    public void Interact(PlayerCharacterLogic playerCharacter)
    {
        damageEntity.Attack(playerCharacter);
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
    }
}