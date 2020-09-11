using UnityEngine;

public class EquipPanel : InformationPanel<EquipData>
{
    [Header("EquipIcons")]
    [SerializeField] private Sprite healthIcon;

    [SerializeField] private Sprite physArmorIcon;
    [SerializeField] private Sprite magicArmorIcon;
    [SerializeField] private Sprite electricArmorIcon;

    [SerializeField] private Sprite accelerateIcon;

    [SerializeField] private Sprite secondJumpIcon;

    [SerializeField] private Sprite destroyIcon;

    [SerializeField] private Sprite shieldIcon;
    [SerializeField] private Sprite waveIcon;

    [SerializeField] private Sprite restoredIcon;

    [SerializeField] private Sprite equipMoneyIcon;
    protected override Sprite IncreaseMoneyIcon => equipMoneyIcon;

    protected override void SetData(EquipData equipData)
    {
        if (equipData.ArmorEntityExist)
        {
            if (equipData.AddedPhysArmor != 0)
                AddElement(physArmorIcon, equipData.AddedPhysArmor.FixeTo(1));

            if (equipData.AddedMagicArmor != 0)
                AddElement(magicArmorIcon, equipData.AddedMagicArmor.FixeTo(1));

            if (equipData.AddedElectricArmor != 0)
                AddElement(electricArmorIcon, equipData.AddedElectricArmor.FixeTo(1));
        }

        if (equipData.HealthEntityExist)
            AddElement(healthIcon, equipData.AddedHealth.FixeTo(1));

        if (equipData.AccelerateEntityExist)
            AddElement(accelerateIcon, equipData.AddedAccelerate.FixeTo(1));

        if (equipData.SecondJumpEntityExist)
            AddElement(secondJumpIcon, $"{equipData.ReloadingSecondJump.FixeTo(1)}s");

        if (equipData.DestroyEntityExist)
            AddElement(destroyIcon, $"{equipData.ReloadingDestroyTime.FixeTo(1)}s");

        if (equipData.ShieldEntityExist)
            AddElement(shieldIcon, $"{equipData.ShieldLivingTime.FixeTo(1)}s ({equipData.ShieldReloadingTime.FixeTo(1)}s)");

        if (equipData.WaveEntityExist)
            AddElement(waveIcon, $"{equipData.WaveLivingTime.FixeTo(1)}s ({equipData.WaveReloadingTime.FixeTo(1)}s)");

        if (equipData.RestoreEntityExist)
            AddElement(restoredIcon, $"{equipData.RestoredHealth.FixeTo(1)} ({equipData.RestoreReloadingTime.FixeTo(1)}s)");
    }
}