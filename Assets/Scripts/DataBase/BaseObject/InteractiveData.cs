using System;
using UnityEngine;

[Serializable]
public class InteractiveData : BaseObjectData
{
    private class DamageEntity
    {
        public float Damage { get; }
        public DamageType DamageType { get; }

        public float DamageTime { get; }
        public int DamageCount { get; }

        public DamageEntity(float damage, DamageType damageType, float damageTime, int damageCount)
        {
            Damage = damage;
            DamageType = damageType;
            DamageTime = damageTime;
            DamageCount = damageCount;
        }
    }

    private class AccelerateMultiplyingEntity
    {
        public float Multiply { get; }
        public float MultiplyingTime { get; }

        public AccelerateMultiplyingEntity(float multiply, float multiplyingTime)
        {
            Multiply = multiply;
            MultiplyingTime = multiplyingTime;
        }
    }

    private const float DEFAULT_DAMAGE_TIME = 0.5f;

    private float ImpactForScore => Mathf.Pow(1.035f, FullImpact);
    private float ImpactForDamage => Mathf.Pow(1.03f, FullImpact);

    public long Score => (long)(score * ImpactForScore);
    private readonly long score;

    public int Tier { get; }

    public float Damage => damageEntity.Damage * ImpactForDamage;
    public DamageType DamageType => damageEntity.DamageType;
    public float DamageTime => damageEntity.DamageTime;
    public int DamageCount => damageEntity.DamageCount;
    public bool DamageEntityExist => damageEntity != null;

    public float UregularDamage => Damage * DEFAULT_DAMAGE_TIME / DamageTime;

    private DamageEntity damageEntity;

    public float AccelerateMultiply => accelerateMultiplyingEntity.Multiply;
    public float AccelerateMultiplyingTime => accelerateMultiplyingEntity.MultiplyingTime;
    public bool AccelerateMultiplyingEntityExist => accelerateMultiplyingEntity != null;

    private AccelerateMultiplyingEntity accelerateMultiplyingEntity;

    public float BlockedTime => blockedTime.Value;
    public bool BlockedTimeEntityExist => blockedTime != null;
    private float? blockedTime;

    public InteractiveData(DynamicParatetrs dynamicParatetrs, string name,
    long openCost, int maxLevel, long increasingObjectCost, long increasingCost,
    long score, int tier)
    : base(dynamicParatetrs, name, openCost, maxLevel, increasingObjectCost, increasingCost)
    {
        this.score = score;
        Tier = tier;
    }

    public InteractiveData SetDamage(float damage, DamageType damageType, float damageTime = DEFAULT_DAMAGE_TIME, int damageCount = 1)
    {
        damageEntity = new DamageEntity(damage, damageType, damageTime, damageCount);
        return this;
    }

    public InteractiveData SetAccelerateMultiply(float multiply, float multiplyingTime)
    {
        accelerateMultiplyingEntity = new AccelerateMultiplyingEntity(multiply, multiplyingTime);
        return this;
    }

    public InteractiveData SetBlockedTime(float blockedTime)
    {
        this.blockedTime = blockedTime;
        return this;
    }
}