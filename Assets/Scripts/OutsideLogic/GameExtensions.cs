using UnityEngine;

public static class GameExtensions
{
    private static readonly float billion = Pow10(9);
    private static readonly float million = Pow10(6);
    private static readonly float thousand = Pow10(3);

    private static float Pow10(int digits) => Mathf.Pow(10, digits);

    public static string Format(this float value)
    {
        if (value >= billion)
            return $"{(value / billion).FixeTo(2)}B";
        if (value >= million)
            return $"{(value / million).FixeTo(2)}M";
        if (value >= thousand)
            return $"{(value / thousand).FixeTo(2)}K";
        else
            return $"{value.FixeTo(2)}";
    }

    public static string Format(this int value) => Format((float)value);
    public static string Format(this long value) => Format((float)value);

    public static EquipData SetAddedHealthTo(this EquipData equipData, out float addedHealth)
    {
        addedHealth = equipData.AddedHealth;
        return equipData;
    }

    public static EquipData SetArmorTo(this EquipData equipData, out float addedPhysArmor, out float addedMagicArmor, out float addedElectricArmor)
    {
        addedPhysArmor = equipData.AddedPhysArmor;
        addedMagicArmor = equipData.AddedMagicArmor;
        addedElectricArmor = equipData.AddedElectricArmor;
        return equipData;
    }

    public static EquipData SetAddedAccelerateTo(this EquipData equipData, out float addedAccelerate)
    {
        addedAccelerate = equipData.AddedAccelerate;
        return equipData;
    }

    public static EquipData SetSecondJumpReloadingTo(this EquipData equipData, out float reloadingTime)
    {
        reloadingTime = equipData.ReloadingSecondJump;
        return equipData;
    }

    public static EquipData SetDestroyTimeTo(this EquipData equipData, out float reloadingDestroyTime)
    {
        reloadingDestroyTime = equipData.ReloadingDestroyTime;
        return equipData;
    }

    public static EquipData SetShieldTo(this EquipData equipData, out float shieldLivingTime, out float shieldReloadingTime)
    {
        shieldLivingTime = equipData.ShieldLivingTime;
        shieldReloadingTime = equipData.ShieldReloadingTime;
        return equipData;
    }

    public static EquipData SetWaveTo(this EquipData equipData, out float waveLivingTime, out float waveReloadingTime)
    {
        waveLivingTime = equipData.WaveLivingTime;
        waveReloadingTime = equipData.WaveReloadingTime;
        return equipData;
    }

    public static EquipData SetRestoredHealthTo(this EquipData equipData, out float restoredHealth, out float restoreReloadingTime)
    {
        restoredHealth = equipData.RestoredHealth;
        restoreReloadingTime = equipData.RestoreReloadingTime;
        return equipData;
    }

    public static InteractiveData SetDamageTo(this InteractiveData interactiveData, out float damage, out DamageType damageType, out float damageTime, out int damageCount)
    {
        damage = interactiveData.Damage;
        damageType = interactiveData.DamageType;
        damageTime = interactiveData.DamageTime;
        damageCount = interactiveData.DamageCount;

        return interactiveData;
    }

    public static InteractiveData SetAccelerateMultiplyingTo(this InteractiveData interactiveData, out float accelerateMultiply, out float accelerateMultiplyingTime)
    {
        accelerateMultiply = interactiveData.AccelerateMultiply;
        accelerateMultiplyingTime = interactiveData.AccelerateMultiplyingTime;

        return interactiveData;
    }

    public static T SetTo<T>(this T dataBaseObject, ISetData<T> setableData)
    where T : BaseObjectData<T>
    {
        setableData.SetData(dataBaseObject);
        return dataBaseObject;
    }
}
