using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DamageEntity : ISetData<InteractiveData>
{
    public Color DamagedColor => damagedColor;
    [SerializeField] private Color damagedColor;

    private float damage;
    private DamageType type;
    public float ColoredTime { get; set; }
    private float damageTime;
    private int damageCount;

    public Collider2D Collider { get; set; }
    public UnityAction<PlayerCharacterLogic> ActionInCoroutineAttack { get; set; }

    public void SetData(InteractiveData interactiveData)
    {
        interactiveData.SetDamageTo(out damage, out type, out damageTime, out damageCount);
        ColoredTime = damageTime;
    }

    public void Attack(PlayerCharacterLogic playerCharacter)
    {
        playerCharacter.AddColorBy(damagedColor, ColoredTime)
        .Damage(damage, type);
    }

    public void ColliderAttack(PlayerCharacterLogic playerCharacter)
    {
        var action = ActionInCoroutineAttack;

        playerCharacter.AddColorBy(damagedColor, ColoredTime)
        .CoroutineDamaging(Collider, damage, type, damageTime, () => action?.Invoke(playerCharacter));
    }

    public void CountAttack(PlayerCharacterLogic playerCharacter)
    {
        var action = ActionInCoroutineAttack;

        playerCharacter.AddColorBy(damagedColor, ColoredTime)
        .CoroutineDamaging(damageCount, damage, type, damageTime, () => action?.Invoke(playerCharacter));
    }
}