using System;
using UnityEngine;

public enum TypeOfEquip
{
    helmet,
    eyer,
    heel
}

public enum Set
{
    Pirate,
    Moto,
    Mexico,
    Builder
}

public class EquipData : BaseObjectData<EquipData>
{
    private class ArmorEntity
    {
        public float AddedPhysArmor { get; }
        public float AddedMagicArmor { get; }
        public float AddedElectricArmor { get; }

        public ArmorEntity(float physArmor, float magicArmor, float electricArmor)
        {
            AddedPhysArmor = physArmor;
            AddedMagicArmor = magicArmor;
            AddedElectricArmor = electricArmor;
        }
    }

    private class ShieldEntity
    {
        public float ShieldLivingTime { get; }
        public float ShieldReloadingTime { get; }

        public ShieldEntity(float shieldLivingTime, float shieldReloadingTime)
        {
            ShieldLivingTime = shieldLivingTime;
            ShieldReloadingTime = shieldReloadingTime;
        }
    }

    private class WaveEntity
    {
        public float WaveLivingTime { get; }
        public float WaveReloadingTime { get; }

        public WaveEntity(float waveLivingTime, float waveReloadingTime)
        {
            WaveLivingTime = waveLivingTime;
            WaveReloadingTime = waveReloadingTime;
        }
    }

    private class RestoreEntity
    {
        public float RestoredHealth { get; }
        public float RestoringReloadingTime { get; }

        public RestoreEntity(float restoredHealth, float restoringReloadingTime)
        {
            RestoredHealth = restoredHealth;
            RestoringReloadingTime = restoringReloadingTime;
        }
    }

    private float ImpactForHealth => Mathf.Pow(1.0263f, FullImpact);
    private float ImpactForArmor => Mathf.Pow(1.0264f, FullImpact);
    private float ImpactForAddedAccelerate => Mathf.Pow(1.2f, ObjectLevelImpact);
    private float ImpactForShield => Mathf.Pow(1.1f, ObjectLevelImpact);
    private float ImpactForWave => Mathf.Pow(1.1f, ObjectLevelImpact);

    public TypeOfEquip TypeOfEquip { get; }
    public Set Set { get; }

    public float AddedHealth => healthEntity.Value * ImpactForHealth;
    public bool HealthEntityExist => healthEntity != null;
    private float? healthEntity;

    public float AddedPhysArmor => armorEntity.AddedPhysArmor * ImpactForArmor;
    public float AddedMagicArmor => armorEntity.AddedMagicArmor * ImpactForArmor;
    public float AddedElectricArmor => armorEntity.AddedElectricArmor * ImpactForArmor;
    public bool ArmorEntityExist => armorEntity != null;
    private ArmorEntity armorEntity;

    public float AddedAccelerate => accelerateEntity.Value * ImpactForAddedAccelerate;
    public bool AccelerateEntityExist => accelerateEntity != null;
    private float? accelerateEntity;

    public float ReloadingSecondJump => secondJumpEntity.Value;
    public bool SecondJumpEntityExist => secondJumpEntity != null;
    private float? secondJumpEntity;

    public float ReloadingDestroyTime => destroyEntity.Value;
    public bool DestroyEntityExist => destroyEntity != null;
    private float? destroyEntity;

    public float ShieldLivingTime => shieldEntity.ShieldLivingTime * ImpactForShield;
    public float ShieldReloadingTime => shieldEntity.ShieldReloadingTime;
    public bool ShieldEntityExist => shieldEntity != null;
    private ShieldEntity shieldEntity;

    public float WaveLivingTime => waveEntity.WaveLivingTime * ImpactForWave;
    public float WaveReloadingTime => waveEntity.WaveReloadingTime;
    public bool WaveEntityExist => waveEntity != null;
    private WaveEntity waveEntity;

    public float RestoredHealth => restoreEntity.RestoredHealth * ImpactForHealth;
    public float RestoreReloadingTime => restoreEntity.RestoringReloadingTime;
    public bool RestoreEntityExist => restoreEntity != null;
    private RestoreEntity restoreEntity;

    public EquipData(DynamicParatetrs dynamicParatetrs, string name,
    long openCost, int maxLevel, long increasingObjectCost, long increasingCost,
    Set set, TypeOfEquip typeOfEquip)
    : base(dynamicParatetrs, name, openCost, maxLevel, increasingObjectCost, increasingCost)
    {
        Set = set;
        TypeOfEquip = typeOfEquip;
    }

    public EquipData SetHealth(float health)
    {
        healthEntity = health;
        return this;
    }

    public EquipData SetArmor(float physArmor, float magicArmor, float electricArmor)
    {
        armorEntity = new ArmorEntity(physArmor, magicArmor, electricArmor);
        return this;
    }

    public EquipData SetAccelerate(float accelerate)
    {
        accelerateEntity = accelerate;
        return this;
    }

    public EquipData SetSecondJump(float reloadingTime)
    {
        secondJumpEntity = reloadingTime;
        return this;
    }

    public EquipData SetDestroyTime(float reloadingTime)
    {
        destroyEntity = reloadingTime;
        return this;
    }

    public EquipData SetShield(float shieldLivingTime, float shieldReloadingTime)
    {
        shieldEntity = new ShieldEntity(shieldLivingTime, shieldReloadingTime);
        return this;
    }

    public EquipData SetWave(float waveLivingTime, float waveReloadingTime)
    {
        waveEntity = new WaveEntity(waveLivingTime, waveReloadingTime);
        return this;
    }

    public EquipData SetRestoredHealth(float restoredHealth, float restoreReloadingTime)
    {
        restoreEntity = new RestoreEntity(restoredHealth, restoreReloadingTime);
        return this;
    }
}