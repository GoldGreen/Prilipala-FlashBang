using System;
using UnityEngine;

public class Fire : MonoBehaviour, IInteractWithPlayerCharacter
{
    public static readonly Quaternion lookRight = Quaternion.Euler(0, 0, 0);
    public static readonly Quaternion lookLeft = Quaternion.Euler(0, 180, 0);

    [SerializeField] private float animationTime;

    private bool lookingRight = true;

    private new Transform transform;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    private void Start()
    {
        CoroutineT.Infinite(FireAnimation, animationTime).Start(this);
    }

    private void FireAnimation()
    {
        transform.rotation = lookingRight ? lookRight : lookLeft;
        lookingRight = !lookingRight;
    }

    public void Interact(PlayerCharacterLogic playerCharacter)
    {

        CoroutineT.Single
        (
            () => playerCharacter
            .Damage(999999, DamageType.magic)
            .AddColorBy(Color.red, 1), 0.2f
        ).Start(this);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        col.GetComponent<IDisposable>()?.Dispose();
    }
}