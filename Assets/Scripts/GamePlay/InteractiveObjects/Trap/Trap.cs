using System;
using UnityEngine;

public class Trap : MonoBehaviour, IHave<IdCode>, ISetData<InteractiveData>, ILinkWithShower, ILinkWithTouchDetector, IInteract, IInteractWithPlayerCharacter, IInteractWithPhysics, IRepoolable, IDisposable, IDestroyedByWave
{
    public IdCode Item => IdCode.Trap;

    [SerializeField] private DamageEntity damageEntity;
    [SerializeField] private float damagedTime = 0.5f;

    [SerializeField] private Sprite spriteOpen;
    [SerializeField] private Sprite spriteClosed;
    [SerializeField] private ParticleSystem effectClosing;
    private Transform particleTransform;

    public TouchDetector TouchDetector { get; set; }
    public EffectShower EffectShower { get; set; }

    private float effectTime = 999999999;

    private static bool subscribed = false;
    private static bool canRemoveDrag = false;
    private int touchedDragedCount = 3;

    private bool TrapClosed => dragCount > 0;
    private int dragCount;

    private IDisposableCollection subscribers = new Disposables();

    private SpriteRenderer spriteRenderer;
    private new Transform transform;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();

        particleTransform = effectClosing.GetComponent<Transform>();
        particleTransform.SetParent(null, false);
    }

    public void SetData(InteractiveData interactiveData)
    {
        interactiveData.SetTo(damageEntity);
    }

    private void Start()
    {
        if (!subscribed)
        {
            subscribed = true;
            TouchDetector.OnDraged.Subscribe(_ => canRemoveDrag = true).AddTo(subscribers);
        }

        TouchDetector.OnDraged.Subscribe
        (
            _ =>
            {
                if (canRemoveDrag && TrapClosed)
                {
                    canRemoveDrag = false;
                    dragCount--;
                    EffectShower.TryRemove(spriteClosed);
                }
            }
        ).AddTo(subscribers);
    }

    private void FixedUpdate()
    {
        particleTransform.position = transform.position;
    }

    public void Interact(PlayerCharacterLogic playerCharacter)
    {
        dragCount = touchedDragedCount;

        CoroutineT.WhileBefore
        (
            () => damageEntity.Attack(playerCharacter),
            () => TrapClosed,
            damagedTime
        ).Start(playerCharacter);
    }

    public void Interact(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    {
        playerRigitBody.gravityScale = 0;

        effectClosing.Play();

        CoroutineT.WhileBefore
        (
            () =>
            {
                control.BlockPlayerAt(damagedTime);
                control.LockTransformBy(transform, damagedTime);
            },
            () => TrapClosed,
            damagedTime,
            () =>
            {
                playerRigitBody.gravityScale = control.WallGravityScale;
                playerRigitBody.velocity = Vector2.zero;
                effectClosing.Stop();
                Dispose();
            }
        ).Start(control);
    }

    public void Interact()
    {
        spriteRenderer.sortingOrder = 7;
        spriteRenderer.sprite = spriteClosed;

        for (int i = 0; i < touchedDragedCount; i++)
        {
            EffectShower.AddOrUpdate(spriteClosed, EffectType.Single, effectTime);
        }
    }

    private void ClearEffect()
    {
        for (; dragCount > 0; dragCount--)
        {
            EffectShower.TryRemove(spriteClosed);
        }

        effectClosing.Stop();
    }

    public void Repool()
    {
        ClearEffect();
        spriteRenderer.sprite = spriteOpen;
        spriteRenderer.sortingOrder = 1;
    }

    public void Dispose()
    {
        ClearEffect();
        gameObject.SetActive(false);
    }

    public void Destroy()
    {
        Dispose();
    }

    private void OnDestroy()
    {
        subscribers.Dispose();
    }
}
