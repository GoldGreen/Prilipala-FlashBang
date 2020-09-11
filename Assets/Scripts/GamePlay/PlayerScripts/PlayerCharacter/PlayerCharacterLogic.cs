using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacterLogic : MonoBehaviour
{
    private Character character;
    private IDisposable subscriber;

    [SerializeField] private PlayerCharacterGraphics graphics;

    [SerializeField] private UnityEvent onDie;

    private new Collider2D collider2D;

    private ColorPipeline colorPipeline;

    public FlagEntity CanGetDamage { get; private set; } = new FlagEntity();

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();

        character = DB.Data.Character;
        subscriber = character.OnDataChanged.Subscribe(x => graphics.SetAnimatedData(x.Health / (float)x.MaxHealth));
    }

    private void Start()
    {
        colorPipeline = new ColorPipeline(GetComponentsInChildren<SpriteRenderer>());
    }

    private void FixedUpdate()
    {
        colorPipeline.DisctinctTime(Time.fixedDeltaTime);
        colorPipeline.UpdateColor();
    }

    public void BlockDamagingAt(float time)
    {
        CanGetDamage.DenyAt(this, time);
    }

    public PlayerCharacterLogic Damage(float damage, DamageType damageType)
    {
        if (CanGetDamage)
        {
            character.Damaged(damage, damageType);

            if (character.Health <= 0)
            {
                onDie?.Invoke();
            }
        }
        return this;
    }

    public PlayerCharacterLogic AddColorBy(Color damagedColor, float time)
    {
        if (CanGetDamage)
        {
            colorPipeline.Add(damagedColor, time);
        }

        return this;
    }

    public void CoroutineDamaging(Collider2D collider2D, float damage, DamageType damageType, float time, UnityAction action = null)
    {
        CoroutineT.WhileBefore
        (
            () =>
            {
                if (CanGetDamage)
                {
                    Damage(damage, damageType);
                    action?.Invoke();
                }
            },
            () => this.collider2D.IsTouching(collider2D),
            time
        ).Start(this);
    }

    public void CoroutineDamaging(int count, float damage, DamageType damageType, float time, UnityAction action = null)
    {
        CoroutineT.ForBefore
        (
            () =>
            {
                if (CanGetDamage)
                {
                    Damage(damage, damageType);
                    action?.Invoke();
                }
            },
            count,
            time
        ).Start(this);
    }

    public void GlobalUpdateHealth()
    {
        character.GlobalUpdate();
    }

    public void LocalUpdateHealth()
    {
        character.LocalUpdate();
    }

    public void AddHealth(float addedHealth)
    {
        character.AddHealth(addedHealth);
    }

    public void RestoreHealth(float restoredHealth)
    {
        character.RestoreHealth(restoredHealth);
    }

    public void AddArmor(float physArmor, float magicArmor, float electricArmor)
    {
        character.AddArmor(physArmor, magicArmor, electricArmor);
    }

    private void OnDestroy()
    {
        subscriber.Dispose();
    }
}